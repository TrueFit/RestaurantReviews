##### clear out the old tables
drop table if exists Review;
drop table if exists Constants;
drop table if exists User;
drop table if exists Restaurant;
drop table if exists RestaurantType;



##### rebuild the needed tables

##### styles of cuisine served
create table RestaurantType
(
	# unique ID for this style, along with its name
	typeID smallint unsigned not null primary key auto_increment,
	name varchar(80) not null
);

##### an individual restaurant
create table Restaurant
(
	# unique ID for restaurant, with it's name and type of cuisine
	restaurantID int unsigned not null primary key auto_increment,
	name varchar(80) not null,
	typeID smallint unsigned not null,
	
	# location data
	address1 varchar(255) not null,
	address2 varchar(255),
	city varchar(40) not null,
	state varchar(40) not null,
	zip varchar(20) not null,
	
	# these don't have to be specified, but aid in searching for nearby restaurants
	latitude decimal(9,6),
	longitude decimal(9,6),
	
	# contact info
	phoneNumber char(12),
	websiteURL varchar(255),

	# flag to hide this restaurant if it is deemed unacceptable
	isApproved enum('Y', 'U', 'N') default 'U',
	
	# foreign keys
	foreign key (typeID) references RestaurantType (typeID) on update cascade
);

##### site/app user
create table User
(
	# unique ID and login info
	userID int unsigned not null primary key auto_increment,
	userName varchar(40) not null,
	email varchar(255) not null,

	# random salt to use in hashing their password
	salt char(64) not null,

	# password is stored as binary representation of a SHA256 hash
	passwordHash binary(32) not null,

	# flag to hide this user if their name/behavior is deemed unacceptable
	isApproved enum('Y', 'U', 'N') default 'U',
	
	# their actual name
	firstName varchar(40),
	lastName varchar(40)
);

##### review for a particular restaurant, created by a specific user
create table Review
(
	# unique ID, along with the restaurant it applies to and the user who created it
	reviewID bigint unsigned not null primary key auto_increment,
	userID int unsigned not null,
	restaurantID int unsigned not null,

	# users are allowed to post multiple reviews of the same establishment, so track when was this one created
	postTime timestamp default current_timestamp,
	
	# the actual text of their review
	reviewText text not null,
	
	# "star"/"$" rankings they provided (I would suggest scale runs from 1-5)
	rating tinyint unsigned,
	price tinyint unsigned,

	# flag to hide this review if it is deemed unacceptable
	isApproved enum('Y', 'U', 'N') default 'U',
	
	# foreign keys
	foreign key (restaurantID) references Restaurant (restaurantID) on delete cascade on update cascade,
	foreign key (userID) references User (userID) on delete cascade on update cascade
);

##### table for any constants we may need
create table Constants
(
	name varchar(255) not null primary key,
	value varchar(255) not null
)