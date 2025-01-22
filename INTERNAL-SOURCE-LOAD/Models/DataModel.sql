-- --------------------------------------------------------
-- Hôte:                         127.0.0.1
-- Version du serveur:           11.6.2-MariaDB - mariadb.org binary distribution
-- SE du serveur:                Win64
-- HeidiSQL Version:             12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Listage de la structure de la base pour trainschedule
CREATE DATABASE IF NOT EXISTS `trainschedule` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci */;
USE `trainschedule`;

-- Listage de la structure de table trainschedule. departures
CREATE TABLE IF NOT EXISTS `departures` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DepartureStationName` varchar(255) NOT NULL,
  `DestinationStationName` varchar(255) NOT NULL,
  `ViaStationNames` text DEFAULT NULL,
  `DepartureTime` datetime NOT NULL,
  `Platform` varchar(50) NOT NULL,
  `Sector` varchar(50) DEFAULT NULL,
  `TrainStationId` int(11) DEFAULT NULL,
  `TrainId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `TrainStationId` (`TrainStationId`),
  KEY `TrainId` (`TrainId`),
  CONSTRAINT `departures_ibfk_1` FOREIGN KEY (`TrainStationId`) REFERENCES `trainstations` (`Id`),
  CONSTRAINT `departures_ibfk_2` FOREIGN KEY (`TrainId`) REFERENCES `trains` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de table trainschedule. trains
CREATE TABLE IF NOT EXISTS `trains` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `G` varchar(50) NOT NULL,
  `L` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de table trainschedule. trainstations
CREATE TABLE IF NOT EXISTS `trainstations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Les données exportées n'étaient pas sélectionnées.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
