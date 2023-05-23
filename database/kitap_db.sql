-- --------------------------------------------------------
-- Sunucu:                       127.0.0.1
-- Sunucu sürümü:                8.0.27 - MySQL Community Server - GPL
-- Sunucu İşletim Sistemi:       Win64
-- HeidiSQL Sürüm:               12.1.0.6537
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- kitap_db için veritabanı yapısı dökülüyor
CREATE DATABASE IF NOT EXISTS `kitap_db` /*!40100 DEFAULT CHARACTER SET latin5 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `kitap_db`;

-- tablo yapısı dökülüyor kitap_db.diller
CREATE TABLE IF NOT EXISTS `diller` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `dilAdi` char(50) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin5;

-- kitap_db.diller: 5 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `diller` DISABLE KEYS */;
INSERT INTO `diller` (`ID`, `dilAdi`) VALUES
	(1, 'Türkçe'),
	(2, 'İngilizce'),
	(3, 'Almanca'),
	(4, 'Arapça'),
	(5, 'Fransızca');
/*!40000 ALTER TABLE `diller` ENABLE KEYS */;

-- tablo yapısı dökülüyor kitap_db.iletisim
CREATE TABLE IF NOT EXISTS `iletisim` (
  `id` int NOT NULL AUTO_INCREMENT,
  `eposta` varchar(100) NOT NULL,
  `konu` varchar(150) NOT NULL,
  `mesaj` varchar(600) NOT NULL,
  `tarihSaat` datetime NOT NULL,
  `ip` char(50) NOT NULL,
  `goruldu` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=latin5;

-- kitap_db.iletisim: 5 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `iletisim` DISABLE KEYS */;
INSERT INTO `iletisim` (`id`, `eposta`, `konu`, `mesaj`, `tarihSaat`, `ip`, `goruldu`) VALUES
	(3, 'test@test.com', 'Konulardan bir konu', 'Mesajlardan bir mesaj', '2022-05-20 10:45:31', '45.45.67.89', 0),
	(4, 'test@test.com', 'yertye', 'dfgsfdg', '2022-05-20 10:48:40', '45.45.67.89', 0),
	(5, 'test@test.com', 'Konulardan bir konu', 'Mesajlardan bir mesaj', '2022-05-20 10:51:01', '45.45.67.89', 0),
	(6, 'tete@sdfg.com', 'retyr', 'rtyrty', '2022-05-25 09:22:29', '192.25.25.36', 0),
	(7, 'tert@dfg.com', 'fasdfasdf', 'dfgsdf gsdfg sdfg', '2022-05-26 14:45:05', '192.25.25.36', 0);
/*!40000 ALTER TABLE `iletisim` ENABLE KEYS */;

-- tablo yapısı dökülüyor kitap_db.kitaplar
CREATE TABLE IF NOT EXISTS `kitaplar` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `adi` char(200) CHARACTER SET latin5 COLLATE latin5_turkish_ci NOT NULL,
  `yazarID` int NOT NULL DEFAULT '1',
  `dilID` int NOT NULL DEFAULT '1',
  `sayfaSayisi` int NOT NULL DEFAULT '1',
  `yayineviID` int NOT NULL DEFAULT '1',
  `ozet` varchar(5000) CHARACTER SET latin5 COLLATE latin5_turkish_ci NOT NULL,
  `yayinTarihi` date NOT NULL,
  `resim` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=201 DEFAULT CHARSET=latin1;

-- kitap_db.kitaplar: 100 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `kitaplar` DISABLE KEYS */;
INSERT INTO `kitaplar` (`ID`, `adi`, `yazarID`, `dilID`, `sayfaSayisi`, `yayineviID`, `ozet`, `yayinTarihi`, `resim`) VALUES
	(1, 'molestie quis lobortis Etiam', 19, 2, 208, 10, 'Curabitur in ante Vivamus erat tempor id sagittis lectus mollis sollicitudin gravida elit sem In Donec eget tincidunt ut tortor tempus eget eu ac consectetur a sit nunc porta nibh leo sit Aenean viverra ornare lobortis mi amet at Vivamus amet eu fringilla', '2023-01-26', '1.jpg'),
	(2, 'mattis accumsan', 28, 1, 484, 10, 'erat amet a pulvinar mollis egestas sollicitudin Quisque in in nunc ut auctor a gravida scelerisque posuere porta nec felis in Curabitur hendrerit suscipit velit lorem euismod eros vel a eget sem nunc diam malesuada bibendum vel Suspendisse Nullam', '2021-03-27', '1.jpg'),
	(3, 'augue quis fermentum tempor id', 20, 5, 194, 1, 'vehicula eget eget aliquet Curabitur non auctor velit id id tempus dictum Maecenas congue sollicitudin ullamcorper gravida cursus diam convallis mattis in quis ut Donec Sed mattis nibh at faucibus commodo non laoreet at arcu eget Aliquam sit purus ac vel Vestibulum erat ut diam laoreet', '2021-12-16', '1.jpg'),
	(4, 'sem non', 39, 4, 568, 3, 'tortor sit congue dui metus turpis id elit mauris lectus quam at Vestibulum erat non velit sem euismod ullamcorper in metus quis', '2023-02-14', '1.jpg'),
	(5, 'mauris', 42, 1, 235, 13, 'vitae Nam leo quam rutrum congue aliquam mi elit Integer tellus facilisis ante ut Praesent amet purus Quisque eget Vivamus odio mattis Quisque velit sed viverra ultrices risus metus posuere', '2022-10-18', '1.jpg'),
	(6, 'lectus', 45, 5, 136, 12, 'augue blandit Sed purus non tincidunt non dictum mauris est volutpat ipsum neque rhoncus lectus id quam viverra sed eleifend Aenean egestas magna volutpat sed neque dolor vel purus tempus mollis dapibus In aliquet Fusce luctus justo orci sit quis vel Aenean elit libero tellus Donec pulvinar tincidunt Quisque magna elit eu ultrices semper justo id arcu malesuada mi', '2023-02-07', '1.jpg'),
	(7, 'blandit dictum elit', 4, 2, 78, 13, 'non nisl hendrerit ullamcorper non et Etiam quis eleifend Morbi Phasellus semper ultrices pulvinar laoreet quis lectus pulvinar ultricies Nulla tortor massa nisl eget eleifend sapien varius Quisque aliquet hendrerit leo nulla facilisis ornare Morbi ante vitae sagittis bibendum ipsum tempor Quisque posuere sed Mauris ac et justo convallis ornare nisi quam magna eleifend', '2021-05-21', '1.jpg'),
	(8, 'ante nec malesuada sodales', 36, 5, 124, 6, 'magna et eget tempor nunc nunc id congue dapibus posuere quis dolor massa Nulla Nam in euismod mi ut lobortis hendrerit Nullam suscipit iaculis Aenean commodo amet Nullam suscipit ac id vulputate sit interdum condimentum vel nulla Integer ut Nulla sed convallis eget eros sit ultricies vitae nisi placerat nisl', '2021-04-07', '1.jpg'),
	(9, 'in diam lectus tempor', 46, 3, 270, 4, 'porta lectus dignissim vel vel Aliquam hendrerit libero eget Aliquam hendrerit sem feugiat id ac vitae a elit metus neque mattis est metus purus non justo vitae vel porta Maecenas nunc Nam et Quisque placerat ultricies nisl dignissim Nunc Cras vitae enim pharetra eu vel', '2022-08-12', '1.jpg'),
	(10, 'Mauris diam', 28, 4, 400, 13, 'egestas tellus eu in a ligula ipsum purus aliquam arcu Praesent malesuada laoreet quam facilisis sit dolor aliquam at aliquet rhoncus dui id rhoncus quam tellus ipsum erat quis eget tempor vehicula blandit pulvinar urna tristique rutrum euismod dapibus neque vitae quis in leo lobortis quis gravida sollicitudin lectus fermentum', '2022-05-09', '1.jpg'),
	(11, 'tortor turpis at', 8, 3, 380, 9, 'cursus lorem a eget bibendum bibendum eget eu lectus Suspendisse eleifend pharetra vitae odio dui elit Sed Vivamus Duis nec ante metus at', '2022-04-03', '1.jpg'),
	(12, 'pharetra ultricies faucibus nulla metus', 23, 4, 200, 9, 'risus nunc lacus libero laoreet Pellentesque ut Praesent ut felis convallis quis lobortis et tempor Phasellus eu id diam hendrerit ligula vitae lorem enim dapibus scelerisque Pellentesque porttitor Nullam suscipit risus egestas laoreet elit hendrerit tincidunt euismod lobortis varius vulputate Donec eget id sit ipsum eget non risus sed iaculis', '2022-12-06', '1.jpg'),
	(13, 'sapien porta nunc ante Integer', 13, 4, 370, 10, 'ac libero tincidunt bibendum eu pulvinar metus quis ac vel nunc amet nisi ipsum Ut at semper a dictum et a quam nisi Nullam id magna nisl nisi id mauris velit mauris posuere', '2022-10-23', '1.jpg'),
	(14, 'felis', 6, 5, 113, 11, 'Nam non dolor tincidunt sit auctor et purus fermentum a a a augue augue nisi nisi eu vel Nulla porta dui Vestibulum eget tellus ac dictum Aliquam id in tristique quis nulla ultricies enim tellus nisi et Phasellus turpis', '2022-10-29', '1.jpg'),
	(15, 'orci convallis ut arcu orci', 48, 3, 462, 14, 'Donec molestie vel erat molestie rutrum Quisque quam ipsum ut auctor ullamcorper scelerisque eget sed lorem mollis tincidunt vulputate orci dignissim vitae tellus iaculis Aliquam vitae a vel laoreet enim hendrerit quis Maecenas lacinia ornare consectetur Nulla magna molestie pulvinar mollis tempor vitae Nulla vitae velit id erat', '2023-01-26', '2.jpg'),
	(16, 'dui malesuada quis mauris id', 32, 5, 327, 15, 'rhoncus ligula Donec vitae ipsum adipiscing id tortor dui vestibulum et Suspendisse at suscipit ac In quis tincidunt Pellentesque nec suscipit sit a at lectus ut dui venenatis mi lectus molestie imperdiet In felis ante sit semper Nulla ultricies dictum diam risus at sit dolor placerat mauris tellus tortor sollicitudin at ut Cras sed nibh aliquam suscipit', '2022-09-29', '2.jpg'),
	(17, 'mi dignissim ligula mollis metus', 35, 5, 598, 6, 'mauris vel Ut ac sit at et magna nisi eleifend dignissim lorem amet non purus ligula mollis gravida ac rutrum', '2022-09-25', '2.jpg'),
	(18, 'sit mauris est fringilla Aliquam', 19, 1, 197, 10, 'sed malesuada vulputate dignissim Nulla a eget nec quis elementum nec posuere scelerisque non id volutpat mattis turpis placerat Curabitur tempus eros nulla suscipit nisi malesuada elit vitae sit at Donec et vestibulum aliquam augue dignissim luctus ultricies rutrum condimentum ultricies egestas sed ultricies eleifend et Nullam tempor adipiscing', '2023-03-15', '2.jpg'),
	(19, 'vitae rhoncus nulla eget', 6, 2, 313, 13, 'eget turpis Pellentesque ultrices leo tellus elit ipsum augue vel orci accumsan lacus elit nisl sed nisl massa amet sed eu pulvinar fermentum augue Curabitur laoreet vestibulum vitae Vivamus quis vel Fusce eget at quam ac venenatis augue Curabitur nunc vel posuere quis Mauris velit mauris a nibh adipiscing Aenean Nam aliquam ut Morbi sollicitudin purus rutrum sodales elit', '2021-12-26', '2.jpg'),
	(20, 'consectetur nulla enim', 21, 4, 552, 3, 'sem purus interdum Curabitur elit suscipit in mollis eros vitae lacinia in eget Proin adipiscing quis turpis amet eu egestas metus vel dapibus bibendum nec facilisis', '2022-01-14', '2.jpg'),
	(21, 'eu aliquet Duis risus mauris', 42, 2, 150, 12, 'et massa ac Proin lectus aliquam libero justo eros vitae sed convallis Praesent Ut a odio lobortis pulvinar elit nec et Pellentesque Mauris Etiam vehicula lorem placerat iaculis metus', '2021-04-16', '2.jpg'),
	(22, 'eleifend ipsum ac', 41, 5, 510, 5, 'massa luctus sit In varius ullamcorper Sed sed est commodo sem massa scelerisque Sed felis porta vel a ante sollicitudin nulla mattis eu ipsum', '2022-09-01', '2.jpg'),
	(23, 'urna id', 37, 2, 150, 11, 'nunc Proin est cursus viverra Donec ante amet sodales adipiscing odio venenatis hendrerit enim varius Pellentesque lectus non augue tincidunt tempor condimentum id Fusce blandit a Sed lacinia Nulla varius sed viverra massa Nunc Proin rutrum tincidunt Integer eget nisl tellus fermentum sed arcu dolor Sed augue est tristique sagittis odio ut pretium sapien velit Fusce at vulputate bibendum velit', '2021-09-23', '2.jpg'),
	(24, 'egestas justo', 37, 2, 498, 2, 'mauris Nunc nunc mollis nec urna mattis eu tortor in tristique imperdiet scelerisque ante vel sem Nunc accumsan arcu Suspendisse nisl nec Mauris neque Donec tortor diam pulvinar id quis eget', '2021-09-13', '2.jpg'),
	(25, 'at', 21, 1, 150, 8, 'odio arcu quis eu mauris amet elit amet tellus nisl sodales tortor Quisque sem quam condimentum consectetur non lobortis eget mauris tempus semper ut viverra lorem tincidunt commodo tempor massa commodo vel molestie gravida', '2021-05-22', '2.jpg'),
	(26, 'tempus pulvinar feugiat Aliquam sagittis', 16, 1, 405, 6, 'facilisis a sem tellus vehicula ut urna sem Proin Pellentesque enim turpis varius velit sodales eu Vivamus vehicula Quisque imperdiet arcu placerat amet et euismod Cras nec sem Sed dapibus ante Morbi sed at quis eros sem dapibus sed', '2021-07-15', '2.jpg'),
	(27, 'tortor ut lectus placerat arcu', 21, 3, 439, 1, 'mi consequat velit placerat mauris Curabitur eget non ullamcorper lorem lectus turpis nunc molestie mauris volutpat eget nisl eu mauris nec leo porta malesuada non sed tincidunt', '2022-03-07', '2.jpg'),
	(28, 'ut ut Etiam Curabitur', 41, 2, 121, 2, 'dignissim in felis lorem suscipit ullamcorper augue id Mauris velit at Integer enim imperdiet mi cursus sed tristique mi laoreet interdum et Aenean in eget sollicitudin', '2022-10-11', '2.jpg'),
	(29, 'Mauris sapien sit in quis', 21, 3, 92, 9, 'Cras Ut in vel sem euismod bibendum tempor lorem sagittis erat vitae ultricies a adipiscing cursus sem Fusce id semper quis mauris bibendum tortor quam placerat', '2021-09-20', '2.jpg'),
	(30, 'felis in ligula', 41, 1, 91, 3, 'condimentum bibendum odio risus diam erat porttitor ut odio sodales sed porttitor blandit dolor dignissim Vestibulum Duis in lectus Ut eget nulla sed lacinia tortor eu tempor sapien ante nisl tincidunt consequat a dictum a Sed', '2022-10-20', '2.jpg'),
	(31, 'facilisis lectus', 5, 2, 536, 4, 'Phasellus metus commodo faucibus arcu mollis metus dolor sed elit sapien velit bibendum Nulla interdum sollicitudin sit tempor lorem lacus lorem sodales vestibulum et enim porttitor lorem ipsum pretium Proin laoreet mollis', '2022-01-06', '2.jpg'),
	(32, 'nunc Curabitur eu mi', 43, 5, 150, 4, 'elit diam sagittis odio urna venenatis sagittis quis ligula mauris ut Maecenas placerat dapibus malesuada tellus consectetur est nec tortor non nunc velit semper leo nulla adipiscing sem convallis id luctus Aenean justo tellus amet elementum euismod non justo bibendum ut tempus nulla aliquam in at Sed elementum sem placerat eget dolor interdum at felis Donec ante bibendum feugiat', '2021-08-22', '2.jpg'),
	(33, 'augue', 40, 5, 383, 7, 'suscipit Cras at nibh cursus vestibulum hendrerit leo varius tortor Etiam non mi eget magna Sed congue semper dui turpis Praesent interdum Phasellus lacus neque et est mauris Praesent Etiam', '2022-05-10', '2.jpg'),
	(34, 'Integer Maecenas', 26, 2, 157, 3, 'imperdiet tincidunt cursus volutpat iaculis purus imperdiet vehicula turpis quis nec ut nisl ullamcorper nunc Curabitur feugiat tincidunt et a eu Nulla congue vel placerat laoreet vitae nulla Sed non venenatis tempor sollicitudin justo interdum condimentum porta sem Fusce nisl mollis Integer interdum mi vel elementum bibendum odio', '2021-05-18', '2.jpg'),
	(35, 'rutrum augue libero imperdiet congue', 40, 4, 155, 10, 'vel malesuada elit nunc at eu aliquet urna sapien dapibus suscipit bibendum porta eleifend lobortis Donec scelerisque velit eget eu mauris eu ut vel Donec posuere varius nec eros luctus Cras iaculis a imperdiet ultricies neque vitae Praesent a semper tristique iaculis nec lectus mollis orci at sit molestie eget aliquam feugiat', '2022-04-19', '3.jpg'),
	(36, 'Nam', 9, 4, 244, 11, 'massa mattis sit eleifend vehicula Curabitur erat id sed Nunc libero Duis nulla purus malesuada lobortis erat Curabitur quis Phasellus lectus tincidunt Fusce', '2022-08-26', '3.jpg'),
	(37, 'commodo vitae tristique neque', 6, 2, 323, 9, 'massa at convallis vehicula Cras elementum feugiat sit placerat convallis porta dolor at euismod lectus metus eget mollis lectus mollis tincidunt quis Morbi mi quam', '2021-07-08', '3.jpg'),
	(38, 'sed Nullam Nullam', 18, 3, 488, 7, 'et dolor vulputate nisi ultricies lorem ipsum eu iaculis cursus gravida rutrum tortor odio volutpat posuere elementum at orci mattis dapibus viverra nisl nibh neque varius malesuada ultrices ipsum rhoncus ultricies iaculis lectus sapien mollis vel nisi feugiat id viverra lacinia', '2021-04-05', '3.jpg'),
	(39, 'non', 22, 2, 429, 3, 'mi ut ante a Sed dolor orci et eu gravida convallis a semper suscipit est amet pulvinar adipiscing cursus felis sodales a nec ut urna vitae tellus pulvinar lorem non commodo porta tortor ullamcorper nunc metus eget turpis felis Praesent tincidunt odio vitae Vestibulum sollicitudin', '2021-12-13', '3.jpg'),
	(40, 'in vestibulum eget', 28, 2, 247, 11, 'accumsan hendrerit fermentum imperdiet purus dapibus elementum Nunc mattis quis gravida tortor mollis blandit est libero adipiscing est sapien Nunc mauris', '2022-05-02', '3.jpg'),
	(41, 'amet nulla', 31, 2, 196, 7, 'dictum Maecenas lacinia ac non amet felis lectus pharetra faucibus sollicitudin varius nec sollicitudin enim varius non eros mi blandit a ultrices elit euismod scelerisque orci', '2021-08-21', '3.jpg'),
	(42, 'ut', 32, 4, 242, 15, 'a lectus ullamcorper Aenean id vitae metus pulvinar est Aliquam imperdiet libero quis congue vel lorem nulla metus libero sit lectus hendrerit Sed sit sit in ut est', '2023-01-24', '3.jpg'),
	(43, 'amet', 12, 4, 261, 4, 'porta feugiat eu et ipsum magna euismod viverra condimentum tortor a et scelerisque Vivamus eu at facilisis tristique eros in Maecenas quam odio imperdiet', '2021-11-27', '3.jpg'),
	(44, 'ut eros libero suscipit eget', 40, 4, 150, 13, 'Donec congue eget adipiscing sed risus lobortis nulla ligula tortor nulla mattis Suspendisse tristique ac malesuada Nulla Aenean eget eleifend Cras sem sapien semper purus purus urna dignissim massa sed molestie Suspendisse congue quam lectus vel bibendum placerat id faucibus tortor at Sed Phasellus diam orci elit varius enim eros nec accumsan tincidunt Proin placerat tellus Sed id', '2022-09-12', '3.jpg'),
	(45, 'viverra Ut mi turpis nisi', 28, 1, 257, 8, 'vitae mattis leo a Praesent sem porta lobortis lectus augue id mi felis eu viverra Mauris et ligula Quisque condimentum dignissim est id nec sagittis quis adipiscing vitae quis rutrum eleifend commodo ligula commodo iaculis accumsan lobortis pulvinar Nullam Nunc vitae nec condimentum malesuada ornare aliquet varius est sem rutrum eu laoreet eu', '2022-07-26', '4.jpg'),
	(46, 'ac lectus a', 27, 4, 62, 10, 'erat eros nunc turpis ultricies laoreet Nunc libero amet non eget felis rutrum elementum auctor lobortis nec tempus sagittis fringilla nisl turpis id justo', '2022-03-16', '4.jpg'),
	(47, 'Nunc Vivamus luctus eleifend Donec', 22, 4, 220, 9, 'rhoncus quam Proin sed id tempor dictum Proin feugiat dictum mattis quam ut Aenean non a auctor tincidunt porta sed purus porta Vestibulum mattis ante arcu ut eu Sed dolor non eget arcu sit viverra commodo in libero arcu Integer libero nunc fringilla odio felis et elementum', '2022-12-08', '4.jpg'),
	(48, 'sollicitudin', 11, 2, 326, 2, 'congue aliquam at porta enim Sed tristique magna suscipit a Donec eget velit consectetur est blandit vehicula vitae Praesent quam mollis urna sapien vitae', '2022-08-22', '4.jpg'),
	(49, 'euismod sed sed mi', 20, 5, 86, 15, 'est eget Praesent leo lorem suscipit ut Nunc a lectus tincidunt quam convallis varius elit et in accumsan diam eu velit ultrices tortor Praesent felis non dui massa ut eget fermentum ullamcorper a dolor erat pharetra et ut est consectetur Vivamus', '2022-12-16', '4.jpg'),
	(50, 'Aliquam auctor', 25, 5, 290, 13, 'dui malesuada tortor tellus faucibus ultrices arcu nisl enim congue nisl orci quis eget Nulla et euismod quis quis pharetra', '2021-06-06', '4.jpg'),
	(51, 'enim metus Sed amet', 42, 3, 319, 13, 'feugiat purus accumsan sit quis est Morbi at elit eleifend Nulla est vel malesuada placerat tortor rutrum in Cras molestie erat justo sodales Nulla lacinia facilisis laoreet at scelerisque consectetur Nulla ut viverra aliquet id vitae hendrerit purus lorem aliquet eros leo auctor ligula arcu dignissim mattis velit id a Aenean', '2022-09-20', '4.jpg'),
	(52, 'tortor', 7, 1, 387, 14, 'auctor mi blandit Etiam lacinia varius Vivamus eget sed vehicula Integer nec magna venenatis In aliquet et turpis elementum elit viverra malesuada erat Vestibulum orci arcu tellus nulla porttitor consectetur elit tempor amet quis sem vehicula posuere eget dignissim tempus Suspendisse id Nunc a Etiam Sed lectus et porttitor convallis malesuada porta a diam', '2021-05-09', '4.jpg'),
	(53, 'sapien Mauris quis egestas vitae', 18, 1, 599, 14, 'ut Pellentesque leo et nunc Donec id ultricies Quisque quis a dignissim interdum leo elementum eget malesuada leo et ut sem at elit sit sodales elementum rutrum ornare Fusce nec venenatis eleifend', '2022-05-31', '4.jpg'),
	(54, 'orci Duis porta dui', 11, 2, 366, 7, 'metus ante felis Aliquam tortor sagittis dictum mauris ultrices Integer vulputate ut dignissim mi ante aliquet quam pretium venenatis eget eleifend velit lorem volutpat mattis mollis lectus Donec faucibus at Curabitur lacus cursus tellus', '2022-09-16', '4.jpg'),
	(55, 'sed id lorem turpis a', 35, 5, 577, 9, 'volutpat eu laoreet Sed Mauris libero blandit Cras in sollicitudin lobortis Sed Nulla vel sed interdum sit viverra blandit sem facilisis fermentum Nunc erat pharetra erat at at lorem sapien est quis sed at faucibus amet magna', '2022-06-14', '4.jpg'),
	(56, 'nec', 18, 4, 578, 13, 'euismod ultrices dui Nulla Mauris vitae vitae scelerisque pulvinar non Sed congue vitae iaculis dui interdum lobortis enim nisi tristique leo purus porta quis dapibus metus orci sit a libero velit accumsan Fusce amet eu bibendum lectus Donec malesuada lobortis risus ac lectus massa rutrum fermentum hendrerit', '2021-07-12', '4.jpg'),
	(57, 'hendrerit quis eget', 20, 4, 400, 6, 'ut et tincidunt Curabitur aliquet velit rhoncus convallis et est vel sem vitae Phasellus massa tempor Nam arcu turpis est Fusce nunc lorem viverra nibh eleifend varius tincidunt nec hendrerit nec nisl non eu in gravida dignissim vitae nisi Donec elit eu sit Mauris fermentum neque ut tempus', '2021-08-05', '4.jpg'),
	(58, 'in', 13, 4, 191, 12, 'tristique blandit laoreet scelerisque justo urna dui turpis lectus aliquam mauris eu sed quam pulvinar id accumsan diam elit quam ante Nunc urna Vivamus eleifend tincidunt interdum leo ipsum luctus pretium turpis auctor fringilla imperdiet blandit et Cras metus Suspendisse at mauris ut Phasellus tristique elit urna Vivamus cursus faucibus feugiat pretium mauris Duis Quisque molestie porta', '2021-08-29', '4.jpg'),
	(59, 'tempor Aliquam lectus at Suspendisse', 37, 1, 196, 10, 'In Donec diam et dictum eleifend et sapien vel Nam elit suscipit porta tellus volutpat varius Ut non sed Donec metus massa commodo Nullam imperdiet eleifend sem semper placerat molestie in vulputate nec eu felis mollis eget mattis Sed feugiat vel vulputate Quisque Morbi dolor ut Pellentesque a Phasellus et ut accumsan mattis Vivamus cursus dui condimentum iaculis quam', '2021-10-28', '4.jpg'),
	(60, 'volutpat tortor dui dolor', 44, 3, 113, 1, 'sodales hendrerit congue condimentum sem tortor sodales Donec pretium posuere Nulla ut fringilla tincidunt purus mollis Curabitur faucibus sed volutpat turpis amet aliquet eu placerat sagittis pharetra ante feugiat non cursus aliquam ut a', '2022-03-24', '4.jpg'),
	(61, 'ac condimentum posuere enim', 43, 4, 410, 5, 'vel Sed sagittis magna blandit diam augue pulvinar egestas felis sem purus ut ante mauris Nulla nisl et sagittis arcu bibendum lectus Aenean', '2021-06-21', '4.jpg'),
	(62, 'ultricies id vitae Aenean varius', 18, 2, 196, 6, 'feugiat in urna eget gravida justo volutpat velit dapibus arcu Proin facilisis amet rutrum porttitor diam semper massa a sit imperdiet rhoncus sapien congue molestie imperdiet in a mi non arcu Etiam velit tincidunt a Nulla Pellentesque neque Duis Quisque erat sem Vivamus nisi Donec amet amet pharetra at non a auctor quis eu erat ut facilisis', '2023-01-15', '4.jpg'),
	(63, 'vitae auctor at', 47, 3, 357, 11, 'ipsum Phasellus Ut adipiscing lectus sed Phasellus purus sodales ac tellus placerat Maecenas semper ac Sed pharetra eget commodo at mauris Aliquam Etiam eget Ut a Phasellus tortor molestie molestie eu Aenean Sed quis purus placerat leo eget', '2021-07-25', '4.jpg'),
	(64, 'non sodales lorem eget mattis', 21, 2, 141, 7, 'tristique ullamcorper nulla libero diam luctus eget libero sed semper enim sagittis in aliquam ligula id in elit ipsum volutpat Integer semper porta lobortis convallis pellentesque elit auctor cursus lectus nunc dolor in Aenean nisi nisl pretium euismod non', '2022-10-13', '4.jpg'),
	(65, 'sollicitudin tristique vehicula', 2, 4, 174, 8, 'eros tortor accumsan augue rutrum id felis eu sed leo eros iaculis felis faucibus nec molestie Mauris nec Nunc tincidunt justo at Aenean in Aenean Sed Aenean elit lectus molestie eget ac nibh et amet nisl urna elit semper Pellentesque eu odio lacinia Praesent in mauris ante vel Donec Donec arcu lectus eget dignissim a quis luctus', '2022-11-14', '5.jpg'),
	(66, 'eu Nulla Curabitur', 26, 1, 204, 3, 'neque a nec Cras bibendum sodales leo nisi ullamcorper risus mauris vitae mollis varius laoreet libero imperdiet id sapien odio eu eu felis dictum nunc vitae lectus vulputate Nulla sit id blandit tristique convallis volutpat felis malesuada Sed congue sapien augue ipsum ultrices Proin blandit porta velit nisi nunc porta tempus sem posuere et In quam sapien eros sed', '2022-05-11', '5.jpg'),
	(67, 'ac risus', 38, 4, 150, 9, 'sapien tincidunt malesuada felis eget lobortis ante vehicula nec Sed Sed sem vitae feugiat venenatis sagittis vitae mauris vehicula nisl ac nibh tellus commodo eu Cras nec semper augue hendrerit enim', '2022-11-17', '5.jpg'),
	(68, 'lectus ullamcorper hendrerit', 42, 1, 179, 14, 'convallis facilisis vel mi tempor semper eu scelerisque laoreet massa sodales quis nisl leo rhoncus orci dolor a justo sed Fusce orci Etiam imperdiet cursus enim', '2022-03-22', '5.jpg'),
	(69, 'sollicitudin velit orci', 46, 3, 159, 12, 'mauris sit Donec vestibulum quam tellus velit elit Sed vulputate dictum enim amet orci non purus arcu Vestibulum velit auctor fringilla sapien at sed mattis', '2021-10-11', '5.jpg'),
	(70, 'lorem eget non', 10, 4, 150, 8, 'et viverra laoreet placerat ut mi Phasellus id tristique in sit Pellentesque amet sed Sed Sed in vel amet Aliquam Nullam odio ut pretium ut', '2022-05-24', '5.jpg'),
	(71, 'sed ante augue mauris nec', 6, 2, 362, 6, 'dignissim sed Etiam Ut eros blandit dignissim Donec lectus tortor aliquet Vestibulum imperdiet metus quis non mi et ut tempor elit Phasellus rhoncus at quis sed tincidunt odio sed mauris Sed erat id sit nulla mi fermentum lorem Nullam arcu vulputate mollis Sed massa dignissim', '2023-01-14', '5.jpg'),
	(72, 'Etiam egestas Sed ac', 13, 2, 82, 15, 'dui mi bibendum tristique Nulla ante sem Curabitur semper lacinia id enim ornare hendrerit ipsum pharetra justo id odio Integer dolor velit sollicitudin pulvinar Integer eu sagittis malesuada pretium pretium eleifend hendrerit orci eu dapibus', '2022-01-06', '6.jpg'),
	(73, 'tortor quis tellus nisi nulla', 28, 4, 335, 8, 'erat ac pretium Sed a eu et quis non eros eget Nullam quis mi neque libero arcu Aenean Suspendisse rutrum dapibus congue hendrerit condimentum', '2022-04-19', '6.jpg'),
	(74, 'lorem', 22, 5, 150, 12, 'dignissim quis non volutpat posuere Curabitur risus justo massa venenatis dapibus ultrices ultricies purus molestie dolor porttitor varius arcu et facilisis vitae tempor risus et sem eget est Duis purus Cras libero quam pharetra iaculis tincidunt in Aenean', '2022-01-08', '6.jpg'),
	(75, 'placerat Phasellus Fusce', 16, 5, 150, 12, 'Fusce vulputate Phasellus nisl velit libero auctor urna dignissim ante Nunc sem lacus diam mollis a ante sagittis lectus rutrum volutpat est vitae consectetur arcu sollicitudin tincidunt magna dui sed libero odio non Curabitur vel Duis metus', '2022-05-12', '6.jpg'),
	(76, 'nunc nulla', 9, 2, 308, 1, 'feugiat nisl Praesent luctus tincidunt Praesent Vestibulum at Fusce quam malesuada semper non euismod nunc in eleifend leo Curabitur ipsum sodales vestibulum quis ante diam gravida elementum Cras Cras augue auctor vulputate', '2022-01-15', '6.jpg'),
	(77, 'euismod varius vitae', 50, 2, 150, 11, 'mauris egestas nulla amet posuere vel in vel nisi leo Duis mollis Sed Quisque consequat in aliquet amet iaculis ut blandit lorem id libero quis nisl Integer Maecenas eros ultricies imperdiet Nunc porttitor rutrum auctor', '2021-08-16', '6.jpg'),
	(78, 'dolor eget at Ut', 20, 1, 535, 9, 'ipsum at elit mi euismod eget lacus Curabitur cursus eu porta fringilla dictum ut ligula aliquet in enim ullamcorper imperdiet dictum et Suspendisse suscipit ante massa lobortis massa ligula sed metus pulvinar risus felis ac magna imperdiet eleifend sem ullamcorper Nunc Duis cursus nec eu risus vel amet', '2021-11-28', '7.jpg'),
	(79, 'tortor', 3, 2, 536, 6, 'interdum placerat eget dolor tempor venenatis tempus scelerisque Praesent vitae feugiat sapien arcu id faucibus a id Vivamus nibh neque eu sapien gravida tellus Sed aliquam a Vestibulum bibendum non tempor vitae Vestibulum cursus Quisque pulvinar sagittis malesuada vehicula', '2021-11-07', '7.jpg'),
	(80, 'ac mollis magna a', 15, 3, 362, 2, 'erat volutpat imperdiet nibh Cras elit nulla convallis tempor at Fusce arcu non pulvinar quam quam non id sollicitudin suscipit diam nibh nec eu Cras dictum sem lacus vestibulum turpis auctor quis Cras vel luctus Cras in enim Proin dignissim purus ut Nunc commodo eu lectus Mauris ante molestie dolor varius', '2022-12-16', '7.jpg'),
	(81, 'venenatis diam', 21, 2, 239, 13, 'sagittis vel at lacinia sed eu eu Maecenas et turpis libero odio quis Nunc tortor Nullam rutrum elit Phasellus non purus leo eu rutrum eget metus Nulla Phasellus mauris condimentum pulvinar pretium elit in Phasellus erat amet erat ut a vitae', '2022-11-21', '7.jpg'),
	(82, 'porta', 36, 2, 263, 9, 'tempus lorem dignissim risus turpis ac arcu vel nisi consectetur ante ullamcorper a risus a vitae ornare ut lacus libero eget a elit ut pulvinar', '2023-02-04', '7.jpg'),
	(83, 'dignissim', 12, 4, 495, 6, 'Aliquam tempor tempus ullamcorper Donec sed non Integer a iaculis fermentum euismod egestas Etiam posuere suscipit massa rhoncus nec commodo pulvinar fermentum vel tellus ullamcorper rhoncus ultricies Sed non eget Duis massa viverra eu sollicitudin mollis amet arcu id tellus a vestibulum nisl Donec Pellentesque convallis id facilisis adipiscing Nulla a suscipit', '2021-10-15', '7.jpg'),
	(84, 'Mauris pharetra', 7, 2, 138, 9, 'congue mollis diam nunc tortor vitae tortor nisi molestie vel erat pretium mi lectus dictum ullamcorper lobortis dolor Curabitur Nulla aliquam a sed mi ipsum amet Sed Curabitur molestie mollis purus', '2023-01-14', '7.jpg'),
	(85, 'nec ut dictum', 19, 4, 269, 6, 'cursus risus Curabitur quam tortor nunc et sem velit imperdiet nec ut Pellentesque egestas condimentum ut sollicitudin vel et vulputate neque facilisis mattis est metus Donec placerat tellus sem Sed sed', '2022-02-10', '7.jpg'),
	(86, 'nisl Mauris euismod', 45, 4, 297, 3, 'sem ut ligula tincidunt sagittis eleifend nunc eu sit lacus vestibulum mollis egestas ornare lectus erat sed fringilla commodo sagittis facilisis ultricies Phasellus tincidunt sodales tristique luctus semper rhoncus leo eget Vivamus Suspendisse tellus Nam amet metus Pellentesque metus ac imperdiet sapien Pellentesque Curabitur quis interdum purus sapien Pellentesque aliquet dignissim dolor sodales Aliquam dolor scelerisque eget non nec semper', '2022-09-10', '7.jpg'),
	(87, 'Phasellus urna non Praesent libero', 44, 4, 363, 12, 'amet elit Maecenas ut nunc dignissim Nam mattis sit sed Etiam dui eleifend nisi ultricies Morbi eget Integer eget congue lorem scelerisque vel hendrerit lorem metus gravida eu Nunc in quis in', '2021-06-21', '7.jpg'),
	(88, 'a Phasellus purus Donec', 49, 3, 353, 14, 'nec tempor fermentum non erat ac in ultricies malesuada suscipit convallis lectus ac at risus congue bibendum sed sed bibendum quis euismod porta', '2022-04-19', '7.jpg'),
	(89, 'vulputate eget velit leo tortor', 24, 5, 207, 12, 'mauris orci Proin tristique turpis libero eget facilisis et nunc adipiscing rutrum quam interdum purus ipsum Vestibulum a nisl interdum quam id aliquet tortor bibendum enim condimentum', '2021-06-02', '8.jpg'),
	(90, 'viverra', 41, 5, 428, 14, 'Aenean risus arcu leo mauris aliquet volutpat imperdiet euismod ante nunc scelerisque dictum sem sagittis vehicula in elit rhoncus vitae sagittis egestas Vestibulum vehicula nulla Fusce placerat sed massa malesuada sit', '2022-05-21', '8.jpg'),
	(91, 'facilisis metus Sed ullamcorper Nunc', 17, 5, 388, 13, 'velit nisi sed suscipit dolor interdum Pellentesque massa Maecenas Etiam neque vehicula augue massa varius placerat non Aliquam metus amet a Aenean at auctor congue pretium mattis nibh fermentum Morbi nisi at placerat nulla eu eleifend Nam vehicula turpis eu Aenean nibh condimentum consectetur vehicula mauris gravida imperdiet nisl sed suscipit hendrerit arcu Suspendisse nisl sapien faucibus erat libero', '2022-10-29', '8.jpg'),
	(92, 'leo tincidunt risus auctor ut', 37, 2, 336, 14, 'in at imperdiet ac nisi sollicitudin quis ligula ornare est est facilisis congue tincidunt pellentesque accumsan fringilla vel sodales nulla urna a elementum sollicitudin ultrices non lacinia erat mauris dignissim leo suscipit tellus imperdiet hendrerit est Suspendisse enim dolor id amet Cras convallis', '2021-05-23', '8.jpg'),
	(93, 'eu et', 50, 5, 106, 2, 'Vestibulum tempor ante nisi nibh porta quis eget eros Nulla scelerisque ligula justo id erat interdum est at Proin sit condimentum', '2022-01-19', '8.jpg'),
	(94, 'at', 12, 3, 107, 5, 'libero mattis turpis suscipit mollis sodales dui arcu augue sit dolor mattis orci arcu nisi odio cursus at mauris euismod ut eu purus ante tincidunt justo a lectus erat quis sapien vitae ipsum pretium urna Vestibulum feugiat dui est arcu rutrum turpis vel et nunc at varius congue Curabitur elit quis Fusce', '2021-04-18', '8.jpg'),
	(95, 'sodales felis sapien quis', 43, 3, 90, 15, 'at mollis Phasellus a imperdiet vestibulum sodales adipiscing Morbi dolor est sit non fermentum ut non Phasellus sed eu nunc facilisis felis neque Aenean erat nisl tempor hendrerit facilisis augue enim faucibus vestibulum lectus luctus scelerisque luctus tempor amet Aenean pretium nibh eget diam dolor eu Vivamus fringilla sagittis metus laoreet dui Cras Morbi magna sapien eros nisi', '2021-08-20', '8.jpg'),
	(96, 'faucibus', 14, 1, 60, 9, 'condimentum sodales et odio Donec lorem ipsum pharetra aliquet iaculis Phasellus ligula egestas libero sollicitudin faucibus fermentum at faucibus libero vitae justo augue Nunc nec ut venenatis quis Duis vulputate sodales nunc sed mattis metus amet mattis est ipsum accumsan hendrerit tempor sed ut pulvinar libero', '2022-09-11', '8.jpg'),
	(97, 'dictum Donec', 41, 2, 403, 10, 'imperdiet euismod ultricies porta id neque lectus diam tincidunt adipiscing et elementum faucibus libero dignissim id Donec vehicula mollis Nullam Aenean commodo nisl magna ipsum cursus imperdiet Aenean Etiam lobortis nisi sit scelerisque ipsum dictum at metus in dolor Pellentesque id', '2022-01-24', '5.jpg'),
	(98, 'est eu', 12, 3, 437, 11, 'at massa commodo arcu quis ut nunc Mauris porta Nam dolor ut sem a ante accumsan mi ullamcorper nulla blandit odio posuere a nulla libero ornare rutrum Cras Morbi velit mattis commodo enim egestas sollicitudin auctor id eros Vestibulum pulvinar quam Etiam sed rutrum feugiat sagittis', '2022-08-01', '8.jpg'),
	(99, 'elementum Sed mollis ut turpis', 4, 3, 272, 6, 'diam orci est scelerisque elementum risus in sit pretium nec ligula nec lobortis suscipit Phasellus amet a ut velit Integer at nulla ac vitae auctor ut fermentum id lorem tortor commodo tellus risus Aenean nibh amet adipiscing auctor eu molestie eu porta gravida feugiat ut metus Mauris augue Aliquam Praesent sed tincidunt', '2022-05-02', '8.jpg'),
	(100, 'laoreet dictum euismod vitae', 25, 2, 150, 2, 'porttitor a at Vestibulum ipsum nisi nibh posuere sem tortor urna malesuada vulputate mauris massa neque feugiat rutrum ultricies porttitor augue id mollis et sodales a et vulputate sagittis tempor Pellentesque est aliquam Sed non non enim interdum venenatis libero fermentum a quis hendrerit ut quis gravida Sed vitae lectus risus eget rhoncus accumsan bibendum quis feugiat sed dictum', '2022-01-03', '5.jpg');
/*!40000 ALTER TABLE `kitaplar` ENABLE KEYS */;

-- tablo yapısı dökülüyor kitap_db.turler
CREATE TABLE IF NOT EXISTS `turler` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `turAdi` char(50) NOT NULL,
  `Sira` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

-- kitap_db.turler: 10 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `turler` DISABLE KEYS */;
INSERT INTO `turler` (`ID`, `turAdi`, `Sira`) VALUES
	(1, 'Roman', 1),
	(2, 'Deneme', 1),
	(3, 'Psikoloji', 1),
	(4, 'Hikaye', 1),
	(5, 'Sosyoloji', 1),
	(6, 'Tarih', 1),
	(7, 'Bilim', 1),
	(8, 'Macera', 1),
	(9, 'Kurgu', 1),
	(10, 'Edebiyat', 1);
/*!40000 ALTER TABLE `turler` ENABLE KEYS */;

-- tablo yapısı dökülüyor kitap_db.turlertokitaplar
CREATE TABLE IF NOT EXISTS `turlertokitaplar` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `turID` int NOT NULL DEFAULT '1',
  `kitapID` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=202 DEFAULT CHARSET=latin1;

-- kitap_db.turlertokitaplar: 200 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `turlertokitaplar` DISABLE KEYS */;
INSERT INTO `turlertokitaplar` (`ID`, `turID`, `kitapID`) VALUES
	(1, 10, 1),
	(2, 9, 2),
	(3, 1, 3),
	(4, 9, 4),
	(5, 5, 5),
	(6, 3, 6),
	(7, 5, 7),
	(8, 8, 8),
	(9, 5, 9),
	(10, 8, 10),
	(11, 5, 11),
	(12, 6, 12),
	(13, 6, 13),
	(14, 3, 14),
	(15, 4, 15),
	(16, 1, 16),
	(17, 10, 17),
	(18, 9, 18),
	(19, 9, 19),
	(20, 9, 20),
	(21, 5, 21),
	(22, 4, 22),
	(23, 8, 23),
	(24, 9, 24),
	(25, 3, 25),
	(26, 10, 26),
	(27, 1, 27),
	(28, 2, 28),
	(29, 7, 29),
	(30, 7, 30),
	(31, 4, 31),
	(32, 10, 32),
	(33, 4, 33),
	(34, 2, 34),
	(35, 7, 35),
	(36, 3, 36),
	(37, 10, 37),
	(38, 3, 38),
	(39, 5, 39),
	(40, 2, 40),
	(41, 6, 41),
	(42, 4, 42),
	(43, 9, 43),
	(44, 1, 44),
	(45, 2, 45),
	(46, 2, 46),
	(47, 1, 47),
	(48, 7, 48),
	(49, 6, 49),
	(50, 1, 50),
	(51, 6, 51),
	(52, 8, 52),
	(53, 9, 53),
	(54, 3, 54),
	(55, 5, 55),
	(56, 7, 56),
	(57, 2, 57),
	(58, 3, 58),
	(59, 7, 59),
	(60, 5, 60),
	(61, 2, 61),
	(62, 4, 62),
	(63, 2, 63),
	(64, 8, 64),
	(65, 5, 65),
	(66, 6, 66),
	(67, 3, 67),
	(68, 5, 68),
	(69, 7, 69),
	(70, 1, 70),
	(71, 2, 71),
	(72, 7, 72),
	(73, 4, 73),
	(74, 7, 74),
	(75, 3, 75),
	(76, 2, 76),
	(77, 6, 77),
	(78, 7, 78),
	(79, 7, 79),
	(80, 1, 80),
	(81, 10, 81),
	(82, 4, 82),
	(83, 7, 83),
	(84, 7, 84),
	(85, 2, 85),
	(86, 7, 86),
	(87, 1, 87),
	(88, 1, 88),
	(89, 7, 89),
	(90, 1, 90),
	(91, 1, 91),
	(92, 7, 92),
	(93, 7, 93),
	(94, 1, 94),
	(95, 10, 95),
	(96, 3, 96),
	(97, 3, 97),
	(98, 1, 98),
	(99, 7, 99),
	(100, 7, 100),
	(102, 5, 100),
	(103, 5, 72),
	(104, 7, 48),
	(105, 10, 64),
	(106, 1, 57),
	(107, 6, 91),
	(108, 6, 34),
	(109, 1, 96),
	(110, 5, 6),
	(111, 9, 24),
	(112, 7, 18),
	(113, 8, 32),
	(114, 2, 17),
	(115, 1, 59),
	(116, 7, 70),
	(117, 7, 25),
	(118, 4, 35),
	(119, 9, 67),
	(120, 2, 85),
	(121, 1, 47),
	(122, 6, 46),
	(123, 5, 2),
	(124, 7, 50),
	(125, 9, 93),
	(126, 5, 81),
	(127, 4, 86),
	(128, 8, 63),
	(129, 9, 32),
	(130, 3, 14),
	(131, 6, 51),
	(132, 5, 37),
	(133, 1, 38),
	(134, 10, 33),
	(135, 6, 21),
	(136, 5, 41),
	(137, 9, 64),
	(138, 10, 40),
	(139, 7, 24),
	(140, 8, 79),
	(141, 7, 68),
	(142, 4, 9),
	(143, 1, 82),
	(144, 4, 80),
	(145, 5, 75),
	(146, 5, 54),
	(147, 5, 1),
	(148, 4, 57),
	(149, 10, 20),
	(150, 5, 4),
	(151, 1, 34),
	(152, 10, 65),
	(153, 5, 9),
	(154, 10, 77),
	(155, 9, 38),
	(156, 10, 66),
	(157, 6, 43),
	(158, 5, 86),
	(159, 2, 38),
	(160, 3, 20),
	(161, 1, 12),
	(162, 9, 79),
	(163, 8, 95),
	(164, 10, 82),
	(165, 4, 43),
	(166, 4, 48),
	(167, 8, 72),
	(168, 7, 33),
	(169, 8, 68),
	(170, 7, 1),
	(171, 9, 55),
	(172, 10, 10),
	(173, 8, 80),
	(174, 10, 3),
	(175, 6, 45),
	(176, 4, 63),
	(177, 10, 25),
	(178, 10, 99),
	(179, 5, 20),
	(180, 9, 51),
	(181, 2, 18),
	(182, 5, 45),
	(183, 10, 49),
	(184, 9, 85),
	(185, 8, 16),
	(186, 2, 53),
	(187, 8, 28),
	(188, 4, 54),
	(189, 2, 76),
	(190, 10, 96),
	(191, 10, 93),
	(192, 3, 88),
	(193, 1, 78),
	(194, 6, 56),
	(195, 8, 70),
	(196, 9, 52),
	(197, 8, 15),
	(198, 9, 66),
	(199, 2, 48),
	(200, 2, 30),
	(201, 10, 14);
/*!40000 ALTER TABLE `turlertokitaplar` ENABLE KEYS */;

-- tablo yapısı dökülüyor kitap_db.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin5;

-- kitap_db.users: 1 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`id`, `username`, `password`) VALUES
	(0, 'koray', 'koray');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

-- tablo yapısı dökülüyor kitap_db.yayinevleri
CREATE TABLE IF NOT EXISTS `yayinevleri` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `yayineviAdi` char(200) NOT NULL,
  `adres` varchar(150) NOT NULL,
  `tel` char(13) NOT NULL,
  `sira` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=16 DEFAULT CHARSET=latin5;

-- kitap_db.yayinevleri: 15 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `yayinevleri` DISABLE KEYS */;
INSERT INTO `yayinevleri` (`ID`, `yayineviAdi`, `adres`, `tel`, `sira`) VALUES
	(1, 'eget ullamcorper', 'Adres', '550 582 90 73', 47),
	(2, 'id tristique', 'Adres', '595 235 79 72', 63),
	(3, 'ac', 'Adres', '568 449 26 78', 45),
	(4, 'Donec', 'Adres', '533 150 00 76', 30),
	(5, 'risus facilisis', 'Adres', '522 395 61 50', 47),
	(6, 'augue Nulla', 'Adres', '517 353 57 76', 32),
	(7, 'purus', 'Adres', '556 237 04 52', 52),
	(8, 'luctus tincidunt', 'Adres', '501 406 51 90', 1),
	(9, 'blandit', 'Adres', '556 629 10 33', 49),
	(10, 'et auctor', 'Adres', '526 216 76 49', 23),
	(11, 'augue Nam', 'Adres', '535 989 58 23', 13),
	(12, 'venenatis non', 'Adres', '510 916 48 03', 86),
	(13, 'et Vestibulum', 'Adres', '566 621 40 86', 38),
	(14, 'nec', 'Adres', '598 197 50 95', 20),
	(15, 'quis', 'Adres', '555 104 22 34', 20);
/*!40000 ALTER TABLE `yayinevleri` ENABLE KEYS */;

-- tablo yapısı dökülüyor kitap_db.yazarlar
CREATE TABLE IF NOT EXISTS `yazarlar` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `adi` char(100) NOT NULL,
  `soyadi` char(100) NOT NULL,
  `dogumTarihi` date NOT NULL,
  `dogumYeri` char(100) NOT NULL,
  `cinsiyeti` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=51 DEFAULT CHARSET=latin5;

-- kitap_db.yazarlar: 50 rows tablosu için veriler indiriliyor
/*!40000 ALTER TABLE `yazarlar` DISABLE KEYS */;
INSERT INTO `yazarlar` (`ID`, `adi`, `soyadi`, `dogumTarihi`, `dogumYeri`, `cinsiyeti`) VALUES
	(1, 'Hıfzullah', 'Çilesiz', '2022-08-03', 'Maldiv Adaları', 1),
	(2, 'İbsan', 'Karacaduralıoğlu', '2023-01-19', 'Maldiv Adaları', 1),
	(3, 'Dilruba', 'Şara', '2022-02-25', 'Nauru', 1),
	(4, 'Vedat', 'Çeribaşı', '2022-12-21', 'Sudan', 1),
	(5, 'Çakıl', 'Çuhacı', '2022-08-17', 'Şili', 0),
	(6, 'Seçgül', 'Eserçelik', '2021-09-03', 'Antigua ve Barbuda', 1),
	(7, 'Ünverdi', 'Olmuş', '2022-09-24', 'Endonezya', 1),
	(8, 'Sadıray', 'Afşin ayyıldız', '2021-03-31', 'Kosta Rika', 0),
	(9, 'Murathan', 'Kartaca', '2021-07-28', 'Nepal', 1),
	(10, 'Mutlualp', 'Örüm', '2021-07-09', 'Bonaire', 1),
	(11, 'Nemika', 'Lenk', '2022-11-01', 'Comoros', 0),
	(12, 'Arkın', 'Kılıç aydın', '2021-11-13', 'El Salvador', 0),
	(13, 'İsmigül', 'Değirmenci', '2023-02-19', 'Moldavya', 1),
	(14, 'Nural', 'Altıneş', '2022-05-09', 'Suriye', 0),
	(15, 'Vahiddin', 'Çaresiz', '2022-12-08', 'Malta', 0),
	(16, 'Kusay', 'Attila', '2022-04-09', 'Türk ve Caicos Adaları', 1),
	(17, 'Ümmü', 'Cansız', '2022-08-11', 'Panama', 0),
	(18, 'Ekmel', 'Tokcan', '2021-08-22', 'Cezayir', 1),
	(19, 'Ayşegül', 'Öncül', '2021-09-29', 'Hindistan', 0),
	(20, 'Devlet', 'Kızıklı', '2021-04-17', 'Yugoslavya', 0),
	(21, 'Kutsi', 'Balaban', '2022-10-29', 'Ukrayna', 1),
	(22, 'Soyselçuk', 'Selçuk', '2022-08-06', 'Bahreyn', 1),
	(23, 'İris', 'Aktaşgil', '2022-07-27', 'Gürcistan', 0),
	(24, 'Tamara', 'Koçyiğit canpolat', '2022-09-12', 'Mauritius', 0),
	(25, 'Sarif', 'Tekelioğlu', '2022-12-10', 'Bosna Hersek', 0),
	(26, 'Çağnur', 'Yuka', '2023-03-05', 'Hindistan', 0),
	(27, 'Utkan', 'Altundal', '2021-05-04', 'Yugoslavya', 0),
	(28, 'Vacibe', 'Güleç', '2021-10-13', 'Sudan', 0),
	(29, 'Mekine', 'Bingül', '2023-01-25', 'İsrail', 1),
	(30, 'Alçin', 'Ağrıdağ', '2022-01-03', 'Çin', 0),
	(31, 'Perkel', 'Taşar şahin', '2023-01-15', 'Katar', 1),
	(32, 'Özan', 'Çinar', '2021-09-03', 'Tayland', 1),
	(33, 'Şahabeddin', 'Sökmen', '2022-11-02', 'Pakistan', 1),
	(34, 'Yiğitcan', 'Yokmaç', '2021-08-24', 'Tunus', 0),
	(35, 'Hüteyn', 'Dizdar', '2022-01-10', 'Gambiya', 0),
	(36, 'Müfide', 'Ünver', '2023-02-06', 'Estonya', 0),
	(37, 'Cemre', 'Komi', '2021-09-24', 'Trinidad ve Tobago', 0),
	(38, 'Turab', 'Baytar papurcu', '2021-05-27', 'İtalya', 1),
	(39, 'Özsungur', 'Bozdemir', '2023-01-13', 'St. Barthelemy', 1),
	(40, 'Sahur', 'Etöz', '2022-08-23', 'Katar', 1),
	(41, 'Asfa', 'Süleymanoğlu', '2023-03-17', 'Ukrayna', 0),
	(42, 'Güler', 'Günver', '2021-05-28', 'İspanya', 0),
	(43, 'Sadberk', 'Duymuş', '2022-02-23', 'Tayland', 1),
	(44, 'Billur', 'Kayacık', '2022-01-17', 'Ekvador', 1),
	(45, 'Şensal', 'Tutal', '2021-10-12', 'Beyaz Rusya', 1),
	(46, 'Canal', 'Göldağ', '2022-10-13', 'Paraguay', 0),
	(47, 'Behra', 'Cankardeş', '2023-02-27', 'Grönland', 1),
	(48, 'İlliyyun', 'Koyun', '2022-11-25', 'Puerto Rico', 0),
	(49, 'Öklü', 'Altunbulak', '2021-07-26', 'Hollanda', 0),
	(50, 'Fadiş', 'Paçacı', '2022-04-16', 'Litvanya', 0);
/*!40000 ALTER TABLE `yazarlar` ENABLE KEYS */;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
