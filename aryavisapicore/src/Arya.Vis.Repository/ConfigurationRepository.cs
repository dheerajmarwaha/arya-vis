using Arya.Exceptions;
using Arya.Storage;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Repositories;
using Arya.Vis.Core.Utils;
using Arya.Vis.Core.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Repository
{
    public class ConfigurationRepository : BaseRepository, IConfigurationRepository
    {
        public readonly ILogger<ConfigurationRepository> _logger;
        public ConfigurationRepository(
            ISqlProvider sqlProvider,
            ILogger<ConfigurationRepository> logger
        ) : base(sqlProvider)
        {
            _logger = logger;
        }

        public async Task<UserConfiguration> GetUserConfigurationAsync(Guid userGuid)
        {
            _logger.LogInformation($"Trying to get the settings for `user`: `{userGuid}`");
            var command = SqlProvider.CreateCommand(Routines.GetUserConfigurations);
            SqlProvider.AddParameterWithValue(command, "vUserGuid", userGuid);
            using (command)
            {
                return await ReadConfigurationAsync(await SqlProvider.ExecuteReaderAsync(command));
            }
        }

        public async Task PatchUserConfigurationAsync(UserConfiguration configuration, Guid userGuid)
        {
            _logger.LogInformation($"Trying to patch the settings for `user`: `{userGuid}`");
            var command = SqlProvider.CreateCommand(Routines.PatchUserConfiguration);
            AddConfigurationParameters(command, configuration, userGuid);
            using (command)
            {
                await SqlProvider.ExecuteNonQueryAsync(command);
            }
        }

        public async Task UpdateUserConfigurationAsync(UserConfiguration configuration, Guid userGuid)
        {
            _logger.LogInformation($"Trying to update the settings for `user`: `{userGuid}`");
            var command = SqlProvider.CreateCommand(Routines.UpdateUserConfiguration);
            AddConfigurationParameters(command, configuration, userGuid);
            using (command)
            {
                await SqlProvider.ExecuteNonQueryAsync(command);
            }
        }

        protected void AddConfigurationParameters(DbCommand command, UserConfiguration configuration, Guid userGuid)
        {
            SqlProvider.AddParameterWithValue(command, "vUserGuid", userGuid);
            SqlProvider.AddParameterWithValue(command, "vDistance", configuration.Distance?.Distance);
            SqlProvider.AddParameterWithValue(command, "vUnit", configuration.Distance?.Unit);
            SqlProvider.AddParameterWithValue(command, "vLanguage", configuration.Language);
            SqlProvider.AddParameterWithValue(command, "vIsAutoLogoutEnabled", configuration.Logout?.IsAutoLogoutEnabled);
            SqlProvider.AddParameterWithValue(command, "vInactiveMinutes", configuration.Logout?.InactiveMinutes);
        }

        protected async Task<UserConfiguration> ReadConfigurationAsync(DbDataReader reader)
        {
            using (reader)
            {
                if (await SqlProvider.ReadAsync(reader))
                {
                    var userConfiguration = new UserConfiguration
                    {
                        Distance = new DistanceModel
                        {
                            Distance = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<string>(reader, "distance"))
                        },
                        Language = await SqlProvider.GetFieldValueAsync<string>(reader, "language"),
                        Logout = new AutoLogout
                        {
                            IsAutoLogoutEnabled = Convert.ToBoolean(await SqlProvider.GetFieldValueAsync<object>(reader, "is_auto_logout_enabled")),
                            InactiveMinutes = Convert.ToInt32(await SqlProvider.GetFieldValueAsync<object>(reader, "inactive_minutes")),
                        },
                        IsManagementUser = Convert.ToBoolean(await SqlProvider.GetFieldValueAsync<object>(reader, "is_management_user")),
                        IsCandidateScoreVisible = Convert.ToBoolean(await SqlProvider.GetFieldValueAsync<object>(reader, "is_candidate_score_visible")),
                        WhitelabelId = await SqlProvider.GetFieldValueAsync<string>(reader, "white_label_id")
                    };
                    var jobLitmit = await SqlProvider.GetFieldValueAsync<object>(reader, "max_job_limit");
                    if (jobLitmit != null)
                    {
                        userConfiguration.JobLimit = Convert.ToInt32(jobLitmit);
                    }
                    var defaultSourceLimit = await SqlProvider.GetFieldValueAsync<object>(reader, "default_source_limit");
                    if (defaultSourceLimit != null)
                    {
                        userConfiguration.DefaultSourceLimit = Convert.ToInt32(defaultSourceLimit);
                    }
                    var maxSourceLimit = await SqlProvider.GetFieldValueAsync<object>(reader, "max_source_limit");
                    userConfiguration.MaxSourceLimit = maxSourceLimit != null ? (int?)Convert.ToInt32(maxSourceLimit) : null;
                    var isAutoSourcingEnabled = await SqlProvider.GetFieldValueAsync<object>(reader, "is_auto_sourcing_enabled");
                    if (isAutoSourcingEnabled != null)
                    {
                        userConfiguration.IsAutoSourcingEnabled = Convert.ToBoolean(isAutoSourcingEnabled);
                    }

                    return userConfiguration;
                }
            }
            return null;
        }

        private async Task<OrganizationConfiguration> ReadOrganizationConfigurationAsync(DbDataReader reader)
        {
            using (reader)
            {
                if (await SqlProvider.ReadAsync(reader))
                {
                    var organizationConfiguration = new OrganizationConfiguration
                    {
                        StackRankType = EnumUtils.GetEnum<StackRankType>(Convert.ToString(await SqlProvider.GetFieldValueAsync<object>(reader, "30StackRankType")))
                    };
                    return organizationConfiguration;
                }
                throw new RepositoryException("ConfigurationRepository", $"No resultset returned on get org configuration");
            }
        }

        public async Task<OrganizationConfiguration> GetOrganizationConfigurationAsync(Guid orgGuid)
        {
            var command = SqlProvider.CreateCommand(Routines.GetOrganizationConfiguration);
            SqlProvider.AddParameterWithValue(command, "vOrgGuid", orgGuid);
            using (command)
            {
                var reader = await SqlProvider.ExecuteReaderAsync(command);
                return await ReadOrganizationConfigurationAsync(reader);
            }
        }

        //public async Task UpdateOrganizationConfigurationAsync(Guid orgGuid, SourceConfiguration sourceConfiguration)
        //{
        //    var command = SqlProvider.CreateCommand(Routines.UpdateOrganizationConfiguration);
        //    SqlProvider.AddParameterWithValue(command, "vOrgGuid", orgGuid);
        //    SqlProvider.AddParameterWithValue(command, "vSourceLimit", sourceConfiguration.SourcingLimit);
        //    SqlProvider.AddParameterWithValue(command, "vIsEnabled", sourceConfiguration.IsEnabled);
        //    SqlProvider.AddParameterWithValue(command, "vPortal", sourceConfiguration.Source.Portal);
        //    using (command)
        //    {
        //        await SqlProvider.ExecuteNonQueryAsync(command);
        //    }
        //}

        private Task<SearchOptions> ReadConfiguredSearchOptionsAsync(DbDataReader reader)
        {
            return null;
        }

        //! TODO : Implementation to be implemented
        //public async Task<SearchOptions> GetConfiguredSearchOptionsAsync(int userId)
        //{
        //    var command = SqlProvider.CreateCommand(Routines.GetConfiguredSearchOptions);
        //    SqlProvider.AddParameterWithValue(command, "vUserId", userId);
        //    using (command)
        //    {
        //        var reader = await SqlProvider.ExecuteReaderAsync(command);
        //        return await ReadConfiguredSearchOptionsAsync(reader);
        //    }
        //}

        //public async Task UpdateOrganizationSourcingTypeConfigurationAsync(int orgId, StackRankType? sourcingType)
        //{
        //    var command = SqlProvider.CreateCommand(Routines.UpdateOrganizationSourcingTypeConfiguration);
        //    SqlProvider.AddParameterWithValue(command, "vOrgId", orgId);
        //    SqlProvider.AddParameterWithValue(command, "v30StackRankType", sourcingType?.ToString());
        //    using (command)
        //    {
        //        await SqlProvider.ExecuteNonQueryAsync(command);
        //    }
        //}

        //public async Task UpdateSourceAuthorizationAsync(int userId, SourceConfiguration sourceConfiguration)
        //{
        //    var command = SqlProvider.CreateCommand(Routines.UpdateSourceAuthorizationStatus);
        //    using (command)
        //    {
        //        SqlProvider.AddParameterWithValue(command, "vUserId", userId);
        //        SqlProvider.AddParameterWithValue(command, "vIsAuthorized", sourceConfiguration.IsAuthorized);
        //        SqlProvider.AddParameterWithValue(command, "vGroup", sourceConfiguration.Source.Group);
        //        await SqlProvider.ExecuteNonQueryAsync(command);
        //    }
        //}

        //public async Task UpdateUserPortalConfigurationAsync(int userId, SourceConfiguration sourceConfiguration)
        //{
        //    var command = SqlProvider.CreateCommand(Routines.UpdateUserPortalConfiguration);
        //    using (command)
        //    {
        //        SqlProvider.AddParameterWithValue(command, "vUserId", userId);
        //        SqlProvider.AddParameterWithValue(command, "vIsEnabled", sourceConfiguration.IsEnabled);
        //        SqlProvider.AddParameterWithValue(command, "vPortal", sourceConfiguration.Source.Portal);
        //        SqlProvider.AddParameterWithValue(command, "vGroup", sourceConfiguration.Source.Group);
        //        await SqlProvider.ExecuteNonQueryAsync(command);
        //    }
        //}

        //public async Task EnableAllSupportedSourcesAsync(int orgId)
        //{
        //    var command = SqlProvider.CreateCommand(Routines.EnableAllSupportedSources);
        //    using (command)
        //    {
        //        SqlProvider.AddParameterWithValue(command, "vOrgId", orgId);
        //        await SqlProvider.ExecuteNonQueryAsync(command);
        //    }
        //}
    }
}

