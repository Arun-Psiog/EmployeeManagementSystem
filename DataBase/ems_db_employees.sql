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
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employees` (
  `EmployeeId` int NOT NULL AUTO_INCREMENT,
  `EmployeeCode` varchar(20) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `DepartmentId` int NOT NULL,
  `JobId` int DEFAULT NULL,
  `IsDeleted` tinyint(1) DEFAULT '0',
  `CreatedBy` int NOT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdatedBy` int DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`EmployeeId`),
  UNIQUE KEY `EmployeeCode` (`EmployeeCode`),
  UNIQUE KEY `Email` (`Email`),
  KEY `DepartmentId` (`DepartmentId`),
  KEY `JobId` (`JobId`),
  CONSTRAINT `employees_ibfk_1` FOREIGN KEY (`DepartmentId`) REFERENCES `departments` (`DepartmentId`),
  CONSTRAINT `employees_ibfk_2` FOREIGN KEY (`JobId`) REFERENCES `bulkuploadjobs` (`JobId`)
) ENGINE=InnoDB AUTO_INCREMENT=66 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (1,'1234','Arun','K','abc@gmail.com','12345678',1,NULL,1,1,'2026-08-27 13:16:39',1,'2026-08-27 13:45:59'),(3,'12345','Arun','K','abc2@gmail.com','12345678',1,NULL,0,1,'2026-08-27 13:47:31',NULL,NULL),(4,'32342','3321','3413123','vali@gmail.com','12345678',2,NULL,0,1,'2026-08-27 22:19:14',NULL,NULL),(46,'EMP1001','Arun','Kumar','arun@gmail.com','9876543201',1,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(47,'EMP1002','John','David','john@gmail.com','9876543202',2,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(48,'EMP1003','Priya','S','priya@gmail.com','9876543203',3,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(49,'EMP1004','Rahul','Kumar','rahul@gmail.com','9876543204',4,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(50,'EMP1005','Sneha','Reddy','sneha@gmail.com','9876543205',1,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(51,'EMP1006','Vikram','M','vikram@gmail.com','9876543206',2,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(52,'EMP1007','Meena','K','meena@gmail.com','9876543207',3,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(53,'EMP1008','Ajay','Singh','ajay@gmail.com','9876543208',4,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(54,'EMP1009','Karthik','R','karthik@gmail.com','9876543209',1,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(55,'EMP1010','Divya','P','divya@gmail.com','9876543210',2,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(56,'EMP1011','Hari','Krishnan','hari@gmail.com','9876543211',3,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(57,'EMP1012','Nisha','M','nisha@gmail.com','9876543212',4,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(58,'EMP1013','Suresh','Babu','suresh@gmail.com','9876543213',1,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(59,'EMP1014','Anitha','R','anitha@gmail.com','9876543214',2,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(60,'EMP1015','Praveen','K','praveen@gmail.com','9876543215',3,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(61,'EMP1016','Keerthi','V','keerthi@gmail.com','9876543216',4,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(62,'EMP1017','Mohan','Raj','mohan@gmail.com','9876543217',1,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(63,'EMP1018','Lakshmi','N','lakshmi@gmail.com','9876543218',2,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(64,'EMP1019','Ramesh','K','ramesh@gmail.com','9876543219',3,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL),(65,'EMP1020','Pooja','S','pooja@gmail.com','9876543220',4,NULL,0,1,'2026-08-28 14:53:01',NULL,NULL);
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
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
