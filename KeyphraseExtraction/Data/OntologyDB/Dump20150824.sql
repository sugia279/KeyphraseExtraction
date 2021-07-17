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
  `is_controlled_term` tinyint(4) NOT NULL,
  `idf` double NOT NULL,
  `length_term` int(11) NOT NULL,
  `node_degree` double NOT NULL,
  `pos_tag` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_term`),
  UNIQUE KEY `stemmed_term_UNIQUE` (`stemmed_term`),
  UNIQUE KEY `term_UNIQUE` (`term`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `candidate_term`
--

LOCK TABLES `candidate_term` WRITE;
/*!40000 ALTER TABLE `candidate_term` DISABLE KEYS */;
INSERT INTO `candidate_term` (`id_term`, `stemmed_term`, `term`, `is_controlled_term`, `idf`, `length_term`, `node_degree`, `pos_tag`) VALUES (1,'applic','applications',1,0,1,0,NULL),(2,'inform','information',1,0,1,0,NULL),(3,'ontolog','ontology\nontologies',1,0,1,0.99,NULL),(4,'software engin','software engineering',1,0,2,0,NULL),(5,'express','expression',1,0,1,0,NULL),(6,'keyword','keywords',1,0,1,3.06,NULL),(7,'semantic rel','semantic relations',1,0,2,0,NULL),(8,'model','model',1,0,1,0,NULL),(9,'recal','recall',1,0,1,0,NULL),(10,'precis','precision',1,0,1,0,NULL),(11,'information retriev','information retrieval',1,0,2,3.51,NULL),(12,'futur','future',1,0,1,0,NULL),(13,'semantic web','semantic web',1,0,2,0.84,NULL),(14,'order','order',1,0,1,0,NULL),(15,'structur','structure\nstructures',1,0,1,0,NULL),(16,'document','documents',1,0,1,0,NULL),(17,'vector space model','vector space model',1,0,3,0,NULL),(18,'form','form',1,0,1,0,NULL),(19,'calcul','calculate',1,0,1,0,NULL),(20,'procedur','procedure',1,0,1,0.84,NULL),(21,'realiz','realize',1,0,1,0,NULL),(22,'record','record',1,0,1,1.38,NULL),(23,'comput','computing\ncomputed',1,0,1,0.84,NULL),(24,'element','elements',1,0,1,0.84,NULL),(25,'compos','composed',1,0,1,0,NULL),(26,'relat','related',1,0,1,0,NULL),(27,'distribut','distribution',1,0,1,0,NULL),(28,'set','set\nsetting',1,0,1,0,NULL),(29,'algorithm','algorithm',1,0,1,1.68,NULL),(30,'concept','concepts',1,0,1,0.84,NULL),(31,'featur','feature',1,0,1,0,NULL),(32,'group','group',1,0,1,0,NULL),(33,'domain ontolog','domain ontology',1,0,2,0,NULL),(34,'gener','generalization',1,0,1,0,NULL),(35,'term','terms',1,0,1,0.84,NULL),(36,'computer sci','computer science',1,0,2,3.75,NULL),(37,'extens','extension',1,0,1,0,NULL),(38,'comment','comment',1,0,1,0,NULL),(39,'properti','properties',1,0,1,0,NULL),(40,'semantic similar','semantic similarity',1,0,2,0,NULL),(41,'method','method',1,0,1,0,NULL),(42,'measur','measuring\nmeasure',1,0,1,0,NULL),(43,'retriev','retrieval',1,0,1,0,NULL),(44,'node','node\nnodes',1,0,1,0.84,NULL),(45,'c','C',1,0,1,0,NULL),(46,'condit','conditions',1,0,1,0,NULL),(47,'root','root',1,0,1,0.84,NULL),(48,'factor','factors',1,0,1,0,NULL),(49,'process','process\nprocessed',1,0,1,0,NULL),(50,'transform','transform',1,0,1,4.2,NULL),(51,'decid','deciding',1,0,1,0,NULL),(52,'list','list',1,0,1,0.69,NULL),(53,'experi','experiment',1,0,1,0,NULL),(54,'analysi','analysis',1,0,1,0,NULL),(55,'verifi','verify',1,0,1,0,NULL),(56,'average precis','average precision',1,0,2,0,NULL),(57,'queri','query',1,0,1,0,NULL),(58,'rank','ranked',1,0,1,0,NULL),(59,'optim','optimize',1,0,1,0,NULL),(60,'commun','communications',1,0,1,0,NULL),(61,'metadata','metadata',1,0,1,0,NULL),(62,'databas','databases',1,0,1,5.97,NULL),(63,'agent','agents',1,0,1,2.22,NULL),(64,'web semant','web semantics',1,0,2,0,NULL),(65,'statistical method','statistical method',0,0,2,0,NULL),(66,'similarity calcul','similarity calculating',0,0,2,0,NULL),(67,'semantic desktop','semantic desktop',0,0,2,0,NULL),(68,'vector retrieval model','vector retrieval model',0,0,3,0,NULL),(69,'semantic vector','semantic vector\nsemantic vectors',0,0,2,0,NULL);
/*!40000 ALTER TABLE `candidate_term` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document`
--

DROP TABLE IF EXISTS `document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `document` (
  `id_document` int(11) NOT NULL AUTO_INCREMENT,
  `id_type` int(11) NOT NULL,
  `id_subject` int(11) DEFAULT NULL,
  `title` text NOT NULL,
  `file_name` text,
  `creator` text,
  `description` text,
  `publisher` text,
  `contributor` text,
  `published_date` varchar(20) DEFAULT NULL,
  `format` varchar(45) DEFAULT NULL,
  `identifier` text,
  `source` text,
  `language` varchar(45) DEFAULT NULL,
  `relation` text,
  `coverage` text,
  `rights` text,
  PRIMARY KEY (`id_document`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document`
--

LOCK TABLES `document` WRITE;
/*!40000 ALTER TABLE `document` DISABLE KEYS */;
INSERT INTO `document` (`id_document`, `id_type`, `id_subject`, `title`, `file_name`, `creator`, `description`, `publisher`, `contributor`, `published_date`, `format`, `identifier`, `source`, `language`, `relation`, `coverage`, `rights`) VALUES (2,1,141,'','A Semantic Vector Retrieval Model for Desktop.pdf','','','','','','pdf','',NULL,'','','','');
/*!40000 ALTER TABLE `document` ENABLE KEYS */;
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
-- Table structure for table `document_terms_relationship`
--

DROP TABLE IF EXISTS `document_terms_relationship`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `document_terms_relationship` (
  `id_document` int(11) NOT NULL,
  `id_term` int(11) NOT NULL,
  `is_keyphrase` tinyint(4) NOT NULL,
  `tf` double NOT NULL,
  `occurence_first_position` double NOT NULL,
  `occurence_position_weight` double NOT NULL,
  PRIMARY KEY (`id_document`,`id_term`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document_terms_relationship`
--

LOCK TABLES `document_terms_relationship` WRITE;
/*!40000 ALTER TABLE `document_terms_relationship` DISABLE KEYS */;
INSERT INTO `document_terms_relationship` (`id_document`, `id_term`, `is_keyphrase`, `tf`, `occurence_first_position`, `occurence_position_weight`) VALUES (2,1,0,0.0416666666666667,0.000600240096038415,0.5),(2,2,0,0.0833333333333333,0.000900360144057623,1),(2,3,1,0.75,0.00240096038415366,9),(2,4,0,0.0416666666666667,0.00270108043217287,0.5),(2,5,0,0.0833333333333333,0.0114045618247299,1),(2,6,0,0.25,0.0117046818727491,3),(2,7,0,0.0416666666666667,0.0135054021608643,0.5),(2,8,0,0.0833333333333333,0.0171068427370948,1),(2,9,0,0.0416666666666667,0.017406962785114,0.5),(2,10,0,0.208333333333333,0.0177070828331333,2.5),(2,11,1,0.0416666666666667,0.0201080432172869,0.5),(2,12,0,0.0416666666666667,0.0219087635054022,0.5),(2,13,0,0.125,0.0225090036014406,1.5),(2,14,0,0.166666666666667,0.0249099639855942,2),(2,15,0,0.125,0.0312124849939976,1.5),(2,16,0,0.5,0.0315126050420168,6),(2,17,1,0.166666666666667,0.031812725090036,2),(2,18,0,0.0416666666666667,0.0327130852340936,0.5),(2,19,0,0.0416666666666667,0.0459183673469388,0.5),(2,20,0,0.0833333333333333,0.0594237695078031,1),(2,21,0,0.0416666666666667,0.0597238895558223,0.5),(2,22,0,0.0416666666666667,0.063625450180072,0.5),(2,23,0,0.541666666666667,0.0639255702280912,6.5),(2,24,0,0.0416666666666667,0.0819327731092437,0.5),(2,25,0,0.0416666666666667,0.0915366146458583,0.5),(2,26,0,0.0833333333333333,0.0966386554621849,1),(2,27,0,0.0416666666666667,0.0990396158463385,0.5),(2,28,0,0.0833333333333333,0.102040816326531,1),(2,29,0,0.0416666666666667,0.1062424969988,0.5),(2,30,0,1,0.108343337334934,12),(2,31,0,0.0416666666666667,0.114045618247299,0.5),(2,32,0,0.0416666666666667,0.122448979591837,0.5),(2,33,0,0.0416666666666667,0.12484993997599,0.5),(2,34,0,0.0416666666666667,0.133553421368547,0.5),(2,35,0,0.0833333333333333,0.142857142857143,1),(2,36,0,0.0416666666666667,0.144357743097239,0.5),(2,37,0,0.0416666666666667,0.145558223289316,0.5),(2,38,0,0.166666666666667,0.155762304921969,2),(2,39,0,0.333333333333333,0.176770708283313,4),(2,40,0,0.125,0.177370948379352,1.5),(2,41,0,0.0833333333333333,0.179171668667467,1),(2,42,0,0.0833333333333333,0.179471788715486,1),(2,43,0,0.125,0.187274909963986,1.5),(2,44,0,0.125,0.194177671068427,1.5),(2,45,0,0.166666666666667,0.1968787515006,2),(2,46,0,0.0416666666666667,0.20438175270108,0.5),(2,47,0,0.208333333333333,0.204981992797119,2.5),(2,48,0,0.0416666666666667,0.228391356542617,0.5),(2,49,0,0.125,0.250900360144058,1.5),(2,50,0,0.0416666666666667,0.254501800720288,0.5),(2,51,0,0.0416666666666667,0.281812725090036,0.5),(2,52,0,0.0833333333333333,0.290516206482593,1),(2,53,0,0.125,0.2953181272509,1.5),(2,54,0,0.125,0.29561824729892,1.5),(2,55,0,0.0416666666666667,0.296218487394958,0.5),(2,56,0,0.0416666666666667,0.316326530612245,0.5),(2,57,0,0.0416666666666667,0.336134453781513,0.5),(2,58,0,0.0416666666666667,0.363145258103241,0.5),(2,59,0,0.0416666666666667,0.365246098439376,0.5),(2,60,0,0.0416666666666667,0.380552220888355,0.5),(2,61,0,0.0416666666666667,0.384453781512605,0.5),(2,62,0,0.0416666666666667,0.385054021608643,0.5),(2,63,0,0.0416666666666667,0.3937575030012,0.5),(2,64,0,0.0416666666666667,0.394657863145258,0.5),(2,65,1,0.0833333333333333,0.0132052821128451,1),(2,66,1,0.0416666666666667,0.0138055222088836,0.5),(2,67,1,0.0416666666666667,0.0198079231692677,0.5),(2,68,1,0.125,0.0204081632653061,1.5),(2,69,1,0.166666666666667,0.0918367346938776,2);
/*!40000 ALTER TABLE `document_terms_relationship` ENABLE KEYS */;
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
INSERT INTO `document_type` (`id_type`, `name`) VALUES (1,'paper');
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

-- Dump completed on 2015-08-24  7:28:00
