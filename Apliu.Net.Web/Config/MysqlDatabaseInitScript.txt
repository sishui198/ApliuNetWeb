CREATE TABLE IF NOT EXISTS TempText (
	TempId varchar(36) NOT NULL DEFAULT '',
	UserId varchar(36) DEFAULT '',
	TextContent varchar(2048) DEFAULT '',
	UpdateTime varchar(25) DEFAULT '',
	IP varchar(15) DEFAULT '',
	TextKey varchar(64) DEFAULT '',
	PRIMARY KEY (TempId)
);

CREATE TABLE IF NOT EXISTS Test (
	Id varchar(36) NOT NULL DEFAULT '',
	CreateTime varchar(25) DEFAULT '',
	Name varchar(128) DEFAULT '',
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS ChatMessage (
	ChatMsgID varchar(36) NOT NULL DEFAULT '',
	UserName varchar(256) DEFAULT '',
	Message varchar(2048) DEFAULT '',
	SendTime varchar(25) DEFAULT '',
	HubConnectionId varchar(128) DEFAULT '',
	IP varchar(15) DEFAULT '',
	PRIMARY KEY (ChatMsgID)
);

CREATE TABLE IF NOT EXISTS ApUserInfo (
	ID varchar(36) NOT NULL DEFAULT '',
	UserId varchar(36) NOT NULL DEFAULT '',
	UserName varchar(32) DEFAULT '',
	RealName varchar(32) DEFAULT '',
	MobileNumber varchar(11) DEFAULT '',
	UserType varchar(16) DEFAULT '',
	Password varchar(255) DEFAULT '',
	CreateTime varchar(25) DEFAULT '',
	Project varchar(64) DEFAULT '',
	IsLocked varchar(1) DEFAULT '',
	PRIMARY KEY (ID)
);

CREATE TABLE IF NOT EXISTS WeChatAutoReply (
	ReplyId varchar(36) NOT NULL DEFAULT '',
	`Match` varchar(256) DEFAULT '',
	Content varchar(2048) DEFAULT '',
	CreateTime varchar(25) DEFAULT '',
	IsFull varchar(1) DEFAULT '',
	IP varchar(15) DEFAULT '',
	PRIMARY KEY (ReplyId)
);