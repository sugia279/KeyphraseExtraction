-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: document_extraction
-- ------------------------------------------------------
-- Server version	5.6.26-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `candidate_term`
--

DROP TABLE IF EXISTS `candidate_term`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `candidate_term` (
  `id_term` int(11) NOT NULL AUTO_INCREMENT,
  `stemmed_term` varchar(200) NOT NULL,
  `term` varchar(200) NOT NULL,
  `is_controlled_term` bit(2) NOT NULL,
  `idf` decimal(10,0) DEFAULT NULL,
  `length_term` int(11) DEFAULT NULL,
  `node_degree` decimal(10,0) DEFAULT NULL,
  `pos_tag` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_term`),
  UNIQUE KEY `stemmed_term_UNIQUE` (`stemmed_term`),
  UNIQUE KEY `term_UNIQUE` (`term`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `candidate_term`
--

LOCK TABLES `candidate_term` WRITE;
/*!40000 ALTER TABLE `candidate_term` DISABLE KEYS */;
/*!40000 ALTER TABLE `candidate_term` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document`
--

DROP TABLE IF EXISTS `document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `document` (
  `id_document` int(11) NOT NULL,
  `id_type` int(11) NOT NULL,
  `id_subject` int(11) DEFAULT NULL,
  `title` text NOT NULL,
  `creator` text,
  `description` text,
  `publisher` text,
  `contributor` text,
  `published date` datetime DEFAULT NULL,
  `format` varchar(45) DEFAULT NULL,
  `identifier` text,
  `source` text,
  `language` varchar(45) DEFAULT NULL,
  `relation` text,
  `coverage` text,
  `rights` text,
  PRIMARY KEY (`id_document`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document`
--

LOCK TABLES `document` WRITE;
/*!40000 ALTER TABLE `document` DISABLE KEYS */;
/*!40000 ALTER TABLE `document` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document_keyphrase`
--

DROP TABLE IF EXISTS `document_keyphrase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `document_keyphrase` (
  `id_document` int(11) NOT NULL,
  `id_term` int(11) NOT NULL,
  `tf` decimal(10,0) DEFAULT NULL,
  `occurence_first_position` decimal(10,0) DEFAULT NULL,
  `occurence_position_weight` decimal(10,0) DEFAULT NULL,
  PRIMARY KEY (`id_document`,`id_term`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document_keyphrase`
--

LOCK TABLES `document_keyphrase` WRITE;
/*!40000 ALTER TABLE `document_keyphrase` DISABLE KEYS */;
/*!40000 ALTER TABLE `document_keyphrase` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document_section`
--

DROP TABLE IF EXISTS `document_section`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `document_section` (
  `id_section` int(11) NOT NULL,
  `section_name` varchar(200) NOT NULL,
  `description` text,
  `weight` decimal(10,0) NOT NULL,
  PRIMARY KEY (`id_section`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document_section`
--

LOCK TABLES `document_section` WRITE;
/*!40000 ALTER TABLE `document_section` DISABLE KEYS */;
/*!40000 ALTER TABLE `document_section` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document_structure`
--

DROP TABLE IF EXISTS `document_structure`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `document_structure` (
  `id_type` int(11) NOT NULL,
  `id_section` int(11) NOT NULL,
  PRIMARY KEY (`id_type`,`id_section`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document_structure`
--

LOCK TABLES `document_structure` WRITE;
/*!40000 ALTER TABLE `document_structure` DISABLE KEYS */;
/*!40000 ALTER TABLE `document_structure` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document_type`
--

DROP TABLE IF EXISTS `document_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `document_type` (
  `id_type` int(11) NOT NULL,
  `name` varchar(200) NOT NULL,
  PRIMARY KEY (`id_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document_type`
--

LOCK TABLES `document_type` WRITE;
/*!40000 ALTER TABLE `document_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `document_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subject`
--

DROP TABLE IF EXISTS `subject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `subject` (
  `idSubject` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(500) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `DirectoryName` varchar(500) NOT NULL,
  `idPredecessor` int(11) NOT NULL,
  PRIMARY KEY (`idSubject`)
) ENGINE=InnoDB AUTO_INCREMENT=215 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subject`
--

LOCK TABLES `subject` WRITE;
/*!40000 ALTER TABLE `subject` DISABLE KEYS */;
INSERT INTO `subject` (`idSubject`, `Description`, `DirectoryName`, `idPredecessor`) VALUES (1,'root','',0),(106,'Cong_nghe_Phan_mem','Cong_nghe_Phan_mem',1),(107,'Ky_thuat_Phan_mem','Ky_thuat_Phan_mem',106),(108,'Phat_trien_phan_mem_','Phat_trien_phan_mem_ma_nguon_mo',107),(109,'Phan_tich_thiet_ke_h','Phan_tich_thiet_ke_he_thong_thong_tin',107),(110,'Phuong_phap_mo_hinh_','Phuong_phap_mo_hinh_hoa',107),(111,'Phat_trien_van_hanh_','Phat_trien_van_hanh_bao_tri_phan_mem',107),(112,'Quan_ly_du_an_Cong_n','Quan_ly_du_an_Cong_nghe_thong_tin',107),(113,'Xu_ly_phan_bo','Xu_ly_phan_bo',107),(114,'Cac_phuong_phap_lap_','Cac_phuong_phap_lap_trinh',107),(115,'Nhap_mon_Cong_nghe_p','Nhap_mon_Cong_nghe_phan_mem',107),(116,'Phat_trien_phan_mem_','Phat_trien_phan_mem_huong_doi_tuong',107),(117,'Dac_ta_hinh_thuc','Dac_ta_hinh_thuc',107),(118,'Kiem_chung_phan_mem','Kiem_chung_phan_mem',107),(119,'Phan_mem_Game','Phan_mem_Game',106),(120,'Nhap_mon_phat_trien_','Nhap_mon_phat_trien_game',119),(121,'Phan_mem_nhung','Phan_mem_nhung',106),(122,'Lap_trinh_nhung_can_','Lap_trinh_nhung_can_ban',121),(123,'He_thong_thong_tin','He_thong_thong_tin',1),(124,'He_thong_thong_tin_D','He_thong_thong_tin_Dia_ly',123),(125,'Nhap_mon_He_thong_th','Nhap_mon_He_thong_thong_tin_dia_ly',124),(126,'Phan_tich_du_lieu','Phan_tich_du_lieu',123),(127,'He_quan_tri_co_so_du','He_quan_tri_co_so_du_lieu_oracle',126),(128,'Phat_trien_ung_dung_','Phat_trien_ung_dung_web',126),(129,'Phan_tich_thiet_ke_h','Phan_tich_thiet_ke_he_thong_thong_tin',126),(130,'Khai_thac_du_lieu','Khai_thac_du_lieu',126),(131,'Nhap_mon_Cong_nghe_p','Nhap_mon_Cong_nghe_phan_mem',126),(132,'Thiet_ke_co_so_du_li','Thiet_ke_co_so_du_lieu',126),(133,'Phan_tich_thiet_ke_h','Phan_tich_thiet_ke_huong_doi_tuong_voi_UML',126),(134,'Lap_trinh_ung_dung_w','Lap_trinh_ung_dung_web_voi_java',126),(135,'He_quan_tri_co_so_du','He_quan_tri_co_so_du_lieu',126),(136,'Lap_trinh_co_so_du_l','Lap_trinh_co_so_du_lieu',126),(137,'Cac_he_co_so_tri_thu','Cac_he_co_so_tri_thuc',126),(138,'He_thong_thong_tin_q','He_thong_thong_tin_quan_ly',123),(139,'Quan_ly_du_an_cong_n','Quan_ly_du_an_cong_nghe_thong_tin',138),(140,'He_thong_thong_tin_k','He_thong_thong_tin_ke_toan',138),(141,'Khoa_hoc_may_tinh','Khoa_hoc_may_tinh',1),(142,'Xu_ly_ngon_ngu_tu_nh','Xu_ly_ngon_ngu_tu_nhien',141),(143,'May_hoc','May_hoc',142),(144,'Nguyen_ly_ngon_ngu_l','Nguyen_ly_ngon_ngu_lap_trinh',142),(145,'Co_so_nganh','Co_so_nganh',141),(146,'Phan_tich_va_Thiet_k','Phan_tich_va_Thiet_ke_thuat_toan',145),(147,'Ly_thuyet_thong_tin','Ly_thuyet_thong_tin',145),(148,'Co_so_lap_trinh','Co_so_lap_trinh',145),(149,'Cau_truc_du_lieu_va_','Cau_truc_du_lieu_va_Giai_thuat',145),(150,'Tinh_toan_mem','Tinh_toan_mem',141),(151,'Cong_nghe_tri_thuc','Cong_nghe_tri_thuc',141),(152,'Tri_tue_nhan_tao','Tri_tue_nhan_tao',151),(153,'Cac_he_co_so_tri_thu','Cac_he_co_so_tri_thuc',151),(154,'Do_hoa_may_tinh','Do_hoa_may_tinh',141),(155,'Do_hoa_may_tinh','Do_hoa_may_tinh',154),(156,'Dai_cuong','Dai_cuong',1),(157,'Tin','Tin',156),(158,'Mang_may_tinh','Mang_may_tinh',157),(159,'Tin_hoc_dai_cuong','Tin_hoc_dai_cuong',157),(160,'Lap_trinh_huong_doi_','Lap_trinh_huong_doi_tuong',157),(161,'Lap_trinh_tren_Windo','Lap_trinh_tren_Window',157),(162,'Co_so_du_lieu','Co_so_du_lieu',157),(163,'Kien_truc_may_tinh','Kien_truc_may_tinh',157),(164,'He_dieu_hanh','He_dieu_hanh',157),(165,'Cau_truc_du_lieu_va_','Cau_truc_du_lieu_va_Giai_thuat',157),(166,'Toan','Toan',156),(167,'Xac_suat_thong_ke','Xac_suat_thong_ke',166),(168,'Cau_truc_roi_rac','Cau_truc_roi_rac',166),(169,'Vat_Ly','Vat_Ly',156),(170,'Ky_thuat_May_tinh','Ky_thuat_May_tinh',1),(171,'He_thong_nhung','He_thong_nhung',170),(172,'Lap_trinh_tren_thiet','Lap_trinh_tren_thiet_bi_di_dong',171),(173,'Lap_trinh_nhung_can_','Lap_trinh_nhung_can_ban',171),(174,'He_thong_chung_thuc_','He_thong_chung_thuc_so',171),(175,'Lap_trinh_he_thong_v','Lap_trinh_he_thong_voi_Java',171),(176,'He_thong_thoi_gian_t','He_thong_thoi_gian_thuc',171),(177,'He_thong_nhung','He_thong_nhung',171),(178,'Xu_ly_song_song_va_h','Xu_ly_song_song_va_he_thong_phan_tan',171),(179,'Kien_truc_may_tinh','Kien_truc_may_tinh',171),(180,'He_thong_so','He_thong_so',171),(181,'He_dieu_hanh','He_dieu_hanh',171),(182,'Vat_ly_dien_tu_Thiet','Vat_ly_dien_tu_Thiet_ke_vi_mach',170),(183,'Thiet_ke_mach','Thiet_ke_mach',182),(184,'Xu_ly_tin_hieu_so','Xu_ly_tin_hieu_so',182),(185,'Thiet_ke_mach_in','Thiet_ke_mach_in',182),(186,'Thiet_ke_vi_mach','Thiet_ke_vi_mach',182),(187,'Thiet_ke_vi_mach_voi','Thiet_ke_vi_mach_voi_HDL',182),(188,'Cac_thiet_bi_va_mach','Cac_thiet_bi_va_mach_dien_tu',182),(189,'Vi_xu_ly_Vi_dieu_khi','Vi_xu_ly_Vi_dieu_khien',182),(190,'DAMH_Thiet_ke_mach','DAMH_Thiet_ke_mach',182),(191,'Ly_thuyet_mach_dien','Ly_thuyet_mach_dien',182),(192,'Robotics_Mechatronic','Robotics_Mechatronics',170),(193,'Dieu_khien_tu_dong_n','Dieu_khien_tu_dong_nang_cao',192),(194,'Logic_mo','Logic_mo',192),(195,'Robot_cong_nghiep','Robot_cong_nghiep',192),(196,'Trinh_bien_dich','Trinh_bien_dich',192),(197,'Mang_may_tinh_va_Tru','Mang_may_tinh_va_Truyen_thong',1),(198,'Lap_trinh_mang','Lap_trinh_mang',197),(199,'Lap_trinh_mang_can_b','Lap_trinh_mang_can_ban',198),(200,'Lap_trinh_ung_dung_M','Lap_trinh_ung_dung_Mang',198),(201,'Quan_tri_va_bao_mat_','Quan_tri_va_bao_mat_mang',197),(202,'An_toan_mang_may_tin','An_toan_mang_may_tinh',201),(203,'He_dieu_hanh_Linux','He_dieu_hanh_Linux',201),(204,'Phan_tich_va_thiet_k','Phan_tich_va_thiet_ke_He_thong',201),(205,'Truyen_thong_mang','Truyen_thong_mang',197),(206,'Tin_hieu_va_mach','Tin_hieu_va_mach',205),(207,'Thiet_bi_mang_va_tru','Thiet_bi_mang_va_truyen_thong_da_phuong_tien',205),(208,'Xu_ly_tin_hieu_so','Xu_ly_tin_hieu_so',205),(209,'Truyen_du_lieu','Truyen_du_lieu',205),(210,'Ly_thuyet_thong_tin','Ly_thuyet_thong_tin',205),(211,'Mang_truyen_thong_va','Mang_truyen_thong_va_di_dong',205),(212,'Dien_tu_cho_cong_ngh','Dien_tu_cho_cong_nghe_thong_tin',205),(213,'Cong_nghe_mang_vien_','Cong_nghe_mang_vien_thong',205),(214,'Cơ sở ngành Hệ thống','co_so_nganh',123);
/*!40000 ALTER TABLE `subject` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-08-21  7:22:33
