CREATE TABLE `mstaryaconfigparameter` (
   `id` int(11) NOT NULL AUTO_INCREMENT,
   `ConfigKey` varchar(250) DEFAULT NULL,
   `ConfigValue` varchar(500) DEFAULT NULL,
   UNIQUE KEY `id_UNIQUE` (`id`)
 ) ENGINE=InnoDB AUTO_INCREMENT=757 DEFAULT CHARSET=latin1