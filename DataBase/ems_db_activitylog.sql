-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: ems_db
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `activitylog`
--

DROP TABLE IF EXISTS `activitylog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activitylog` (
  `LogId` bigint NOT NULL AUTO_INCREMENT,
  `UserId` int DEFAULT NULL,
  `Action` varchar(100) NOT NULL,
  `EntityName` varchar(50) NOT NULL,
  `EntityId` int DEFAULT NULL,
  `Details` text,
  `IPAddress` varchar(45) DEFAULT NULL,
  `Timestamp` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`LogId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activitylog`
--

LOCK TABLES `activitylog` WRITE;
/*!40000 ALTER TABLE `activitylog` DISABLE KEYS */;
INSERT INTO `activitylog` VALUES (1,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 11:38:14'),(2,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 11:42:14'),(3,3,'LOGIN_SUCCESS','Users',3,'User logged in.','::1','2026-08-28 13:44:26'),(4,NULL,'LOGIN_FAILED','Users',1,'Incorrect password for admin','::1','2026-08-28 13:55:23'),(5,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 13:55:49'),(6,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 14:10:09'),(7,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 14:13:10'),(8,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 14:14:30'),(9,3,'LOGIN_SUCCESS','Users',3,'User logged in.','::1','2026-08-28 14:39:52'),(10,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 14:46:59'),(11,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 14:59:37'),(12,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 15:01:18'),(13,1,'LOGIN_SUCCESS','Users',1,'User logged in.','::1','2026-08-28 15:51:00'),(14,3,'LOGIN_SUCCESS','Users',3,'User logged in.','::1','2026-08-28 15:51:14'),(15,NULL,'LOGIN_FAILED','Users',3,'Incorrect password for Arun','::1','2026-08-28 15:51:56'),(16,NULL,'LOGIN_FAILED','Users',3,'Incorrect password for Arun','::1','2026-08-28 15:58:32'),(17,NULL,'LOGIN_FAILED','Users',3,'Incorrect password for Arun','::1','2026-08-28 15:58:38'),(18,3,'LOGIN_SUCCESS','Users',3,'User logged in.','::1','2026-08-28 15:58:54');
/*!40000 ALTER TABLE `activitylog` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-08-28 16:52:16
