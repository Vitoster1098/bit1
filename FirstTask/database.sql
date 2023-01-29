-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Янв 27 2023 г., 22:48
-- Версия сервера: 5.7.31
-- Версия PHP: 7.4.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `wpf`
--
CREATE DATABASE IF NOT EXISTS `wpf` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `wpf`;

-- --------------------------------------------------------

--
-- Структура таблицы `apartments`
--

DROP TABLE IF EXISTS `apartments`;
CREATE TABLE IF NOT EXISTS `apartments` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `area` float(15,2) NOT NULL,
  `house_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `house_id` (`house_id`)
) ENGINE=MyISAM AUTO_INCREMENT=65 DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `apartments`
--

INSERT INTO `apartments` (`id`, `area`, `house_id`) VALUES
(1, 71.12, 0),
(2, 89.00, 0),
(3, 102.12, 0),
(4, 65.00, 1),
(5, 33.00, 2),
(6, 45.33, 2),
(7, 67.67, 2),
(8, 71.50, 3),
(9, 66.89, 4),
(10, 102.00, 5),
(11, 77.30, 6),
(12, 88.00, 6),
(13, 65.00, 7),
(14, 71.50, 7),
(15, 109.72, 26),
(16, 53.72, 12),
(17, 63.55, 27),
(18, 39.50, 30),
(19, 53.82, 2),
(20, 59.76, 20),
(21, 61.98, 6),
(22, 82.50, 25),
(23, 39.42, 22),
(24, 119.01, 19),
(25, 51.47, 11),
(26, 126.29, 12),
(27, 86.68, 32),
(28, 53.70, 23),
(29, 94.91, 10),
(30, 89.77, 18),
(31, 46.21, 22),
(32, 94.84, 20),
(33, 45.02, 25),
(34, 33.24, 16),
(35, 43.59, 1),
(36, 73.39, 2),
(37, 124.95, 25),
(38, 45.06, 30),
(39, 79.87, 16),
(40, 98.83, 0),
(41, 71.35, 0),
(42, 127.03, 9),
(43, 33.53, 18),
(44, 75.70, 19),
(45, 127.13, 16),
(46, 100.45, 21),
(47, 71.26, 29),
(48, 91.31, 16),
(49, 112.14, 21),
(50, 103.55, 13),
(51, 107.12, 6),
(52, 59.91, 27),
(53, 70.37, 0),
(54, 120.99, 6),
(55, 120.20, 14),
(56, 107.30, 21),
(57, 102.25, 16),
(58, 98.06, 30),
(59, 112.53, 21),
(60, 71.99, 21),
(61, 42.41, 11),
(62, 62.44, 9),
(63, 117.27, 21),
(64, 59.78, 13);

-- --------------------------------------------------------

--
-- Структура таблицы `cities`
--

DROP TABLE IF EXISTS `cities`;
CREATE TABLE IF NOT EXISTS `cities` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `cities`
--

INSERT INTO `cities` (`id`, `name`) VALUES
(1, 'Москва'),
(2, 'Ярославль'),
(3, 'Санкт-Петербург'),
(4, 'Ростов'),
(5, 'Кострома'),
(6, 'Рыбинск'),
(7, 'Уфа'),
(8, 'Тверь');

-- --------------------------------------------------------

--
-- Структура таблицы `houses`
--

DROP TABLE IF EXISTS `houses`;
CREATE TABLE IF NOT EXISTS `houses` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `number` varchar(50) NOT NULL,
  `street_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `street_id` (`street_id`)
) ENGINE=MyISAM AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `houses`
--

INSERT INTO `houses` (`id`, `number`, `street_id`) VALUES
(1, '27', 0),
(2, '12', 0),
(3, '34', 0),
(4, '53', 0),
(5, '34/21', 1),
(6, '1', 1),
(7, '99', 1),
(8, '69', 1),
(9, '51', 2),
(10, '27', 2),
(11, '33', 2),
(12, '11', 2),
(13, '32', 3),
(14, '220', 3),
(15, '110', 3),
(16, '14', 3),
(17, '21', 4),
(18, '310', 4),
(19, '512', 4),
(20, '112', 4),
(21, '1', 5),
(22, '55', 5),
(23, '72', 5),
(24, '45/12', 5),
(25, '90', 6),
(26, '44', 6),
(27, '121', 6),
(28, '202', 6),
(29, '56', 7),
(30, '65', 7),
(31, '99', 7),
(32, '1', 7);

-- --------------------------------------------------------

--
-- Структура таблицы `streets`
--

DROP TABLE IF EXISTS `streets`;
CREATE TABLE IF NOT EXISTS `streets` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `city_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `city_id` (`city_id`)
) ENGINE=MyISAM AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `streets`
--

INSERT INTO `streets` (`id`, `name`, `city_id`) VALUES
(1, 'улица Авангардная', 1),
(2, 'улица Бойцова', 1),
(3, 'улица Авангардная', 1),
(4, 'улица Викторенко', 1),
(5, 'улица Ленина', 2),
(6, 'улица Пушкина', 2),
(7, 'проспект Октября', 2),
(8, 'улица Некрасова', 2),
(9, 'улица Бассейная', 3),
(10, 'Волхонское шоссе', 3),
(11, 'улица Состязаний', 3),
(12, 'улица Гуммолосаровская', 3),
(13, 'улица Коммунистическая', 4),
(14, 'улица Суворова', 4),
(15, 'улица Власская', 4),
(16, 'улица Преображенская', 4),
(17, 'улица Варшавская', 5),
(18, 'проспект Мира', 5),
(19, 'улица Тминская', 5),
(20, 'улица Бортавская', 5),
(21, 'улица Рыбская', 6),
(22, 'улица Водная', 6),
(23, 'улица Оземская', 6),
(24, 'улица Наземская', 6),
(25, 'улица Братская', 7),
(26, 'улица Сестринская', 7),
(27, 'улица Третьяковская', 7),
(28, 'улица Соколовская', 7),
(29, 'улица Матвеевская', 7);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
