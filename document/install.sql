CREATE TABLE IF NOT EXISTS CacheKey(
 Id INT NOT NULL AUTO_INCREMENT,
  Name VARCHAR(100) NOT NULL ,
  Description VARCHAR(200),
  AppId INT NOT NULL,
  PRIMARY KEY (Id)
);


CREATE TABLE IF NOT EXISTS AppInfo(
 Id INT NOT NULL AUTO_INCREMENT,
  Name VARCHAR(100) NOT NULL ,
  Description VARCHAR(200),
  Identity VARCHAR(36),
  Type INT NOT NULL,
  ConnectionString VARCHAR(200),
  PRIMARY KEY (Id)
);
insert into cachemanager.appinfo(Name,Description,Identity,Type,ConnectionString) values('Noah Cms','cms系统','',0,'localhost');
insert into cachemanager.cachekey(Name,Description,AppId) values('Noah:cms:{A-z}{0-1000}','',1);