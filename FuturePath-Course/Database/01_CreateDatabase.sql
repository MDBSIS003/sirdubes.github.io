-- Explicit setup only. Existing databases are never dropped or reset.
USE master;
GO
IF DB_ID(N'FuturePathStudentAdvisingSystem') IS NULL
 CREATE DATABASE FuturePathStudentAdvisingSystem;
GO
