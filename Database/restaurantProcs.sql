##### Normal user procedures
drop function if exists GetDistanceInMiles;
drop procedure if exists AddRestaurant;
drop procedure if exists AddReview;
drop procedure if exists EditReview;
drop procedure if exists AddUser;
drop procedure if exists DeleteReview;

##### Admin procedures
drop procedure if exists AdminEditRestaurant;
drop procedure if exists AdminEditReview;
drop procedure if exists AdminEditUser;
drop procedure if exists AdminAddRestaurantType;
drop procedure if exists AdminEditRestaurantType;





delimiter //

##### Normal user procedures
# This function takes the latitude and longitude of 2 locations and computes the distance between them,
# using a formula detailed at http://mathforum.org/library/drmath/view/51711.html
create function GetDistanceInMiles( _latitude1  decimal(9,6) ,
                                    _longitude1 decimal(9,6) ,
                                    _latitude2  decimal(9,6) , 
                                    _longitude2 decimal(9,6) )
returns decimal(6,2)
begin
	declare _lat1 decimal(9,6);
	declare _long1 decimal(9,6);
	declare _lat2 decimal(9,6);
	declare _long2 decimal(9,6);

	# convert the latitude and longitudes into radians
	select _latitude1 * PI() / 180.0 into _lat1;
	select _longitude1 * PI() / 180.0 into _long1;
	select _latitude2 * PI() / 180.0 into _lat2;
	select _longitude2 * PI() / 180.0 into _long2;

	# calculate the distance across the curved surface of the earth
	return Acos(Cos(_lat1) * Cos(_long1) * Cos(_lat2) * Cos(_long2) + 
	            Cos(_lat1) * Sin(_long1) * Cos(_lat2) * Sin(_long2) + 
	            Sin(_lat1) * Sin(_lat2)) * 3959.0;
end; //

# This procedure adds a new restaurant to the database.  Required arguments are name, type, address line 1,
# city, state, and zip.  Optional arguments are address line 2, latitude, longitude, phoneNumber, and 
# websiteURL.
create procedure AddRestaurant( _name        varchar(80)       ,
                                _type        smallint unsigned ,
                                _addr1       varchar(255)      ,
                                _addr2       varchar(255)      ,
                                _city        varchar(40)       ,
                                _state       varchar(40)       ,
                                _zip         varchar(20)       ,
                                _latitude    decimal(9,6)      ,
                                _longitude   decimal(9,6)      ,
                                _phoneNumber char(12)          ,
                                _websiteURL  varchar(255)      )
begin
	# insert the new row into the database
	insert into Restaurant (name, typeID, address1, address2, city, state, zip, latitude, longitude, phoneNumber,
		websiteURL) values (_name, _type, _addr1, _addr2, _city, _state, _zip, _latitude, _longitude, 
		_phoneNumber, _websiteURL);
end; //

# This procedure adds a new review to the database.  Required arguments are userID, passwordHash, restaurantID, and
# text.  Optional arguments are rating and price.
create procedure AddReview( _userID       int unsigned     ,
                            _passwordHash binary(32)       ,
                            _restaurantID int unsigned     ,
                            _text         text             ,
                            _rating       tinyint unsigned ,
                            _price        tinyint unsigned )
begin
	declare _pwTest binary(32) default "test";
	declare _pwCheck binary(32) default "check";

	# verify they sent in the right password
	select unhex(sha2(concat(salt, hex(_passwordHash)), 256)), passwordHash into _pwTest, _pwCheck from User 
		where userID=_userID;
	if _pwTest = _pwCheck then
		# insert the new row into the database
		insert into Review (userID, restaurantID, reviewText, rating, price) values (_userID, _restaurantID, _text, 
			_rating, _price);
	end if;
end; //

# This procedure edits a review already in the database.  Required arguments are userID, passwordHash, reviewID, and
# text.  Optional arguments are rating and price.
create procedure EditReview( _userID       int unsigned     ,
                             _passwordHash binary(32)       ,
                             _reviewID     int unsigned     ,
                             _text         text             ,
                             _rating       tinyint unsigned ,
                             _price        tinyint unsigned )
begin
	declare _pwTest binary(32) default "test";
	declare _pwCheck binary(32) default "check";

	# verify they sent in the right password
	select unhex(sha2(concat(salt, hex(_passwordHash)), 256)), passwordHash into _pwTest, _pwCheck from Review 
		join User using (userID) where userID=_userID and reviewID=_reviewID;
	if _pwTest = _pwCheck then
		# update the row in the database
		update Review set reviewText=_text, isApproved='U', rating=_rating, price=_price where reviewID=_reviewID;
	end if;
end; //

# This procedure adds a new user to the database.  Required arguments are username, email, salt, and passwordHash.
# Optional arguments are firstName and lastName.
create procedure AddUser( _username     varchar(40)  ,
                          _email        varchar(255) ,
                          _salt         char(64)     ,
                          _passwordHash binary(32)   ,
                          _firstName    varchar(40)  ,
                          _lastName     varchar(40)  )
begin
	# insert the new row into the database
	insert into User (userName, email, salt, passwordHash, firstName, lastName) values (_username, _email, _salt, 
		_passwordHash, _firstName, _lastName);
end; //

# This procedure deletes a review from the database.  Required arguments are reviewID and userID.
create procedure DeleteReview( _userID   int unsigned    ,
                               _passwordHash binary(32)  ,
                               _reviewID bigint unsigned )
begin
	declare _pwTest binary(32) default "test";
	declare _pwCheck binary(32) default "check";

	# verify they sent in the right password
	select unhex(sha2(concat(salt, hex(_passwordHash)), 256)), passwordHash into _pwTest, _pwCheck from User 
		where userID=_userID;
	if _pwTest = _pwCheck then
		# delete it from the database, making sure the right user is trying to delete it
		delete from Review where reviewID=_reviewID and userID=_userID;
	end if;
end; //

##### Admin procedures
# This procedure edits a restaurant existing in the database.  Required arguments are restaurantID, isApproved, 
# name, type, address line 1, city, state, and zip.  Optional arguments are address line 2, latitude, longitude, 
# phoneNumber, and websiteURL.
create procedure AdminEditRestaurant( _restaurantID int unsigned      ,
                                      _isApproved   char(1)           ,
                                      _name         varchar(80)       ,
                                      _type         smallint unsigned ,
                                      _addr1        varchar(255)      ,
                                      _addr2        varchar(255)      ,
                                      _city         varchar(40)       ,
                                      _state        varchar(40)       ,
                                      _zip          varchar(20)       ,
                                      _latitude     decimal(9,6)      ,
                                      _longitude    decimal(9,6)      ,
                                      _phoneNumber  char(12)          ,
                                      _websiteURL   varchar(255)      )
begin
	# edit the row in the database
	update Restaurant set name=_name, address1=_addr1, address2=_addr2, city=_city, state=_state, zip=_zip, 
		latitude=_latitude, longitude=_longitude, phoneNumber=_phoneNumber, websiteURL=_websiteURL,
		isApproved=_isApproved where restaurantID=_restaurantID;
end; //

# This procedure edits a review in the database.  Required arguments are reviewID, isApproved, and text.
create procedure AdminEditReview( _reviewID   bigint unsigned ,
                                  _isApproved char(1)         ,
                                  _text       text            )
begin
	# edit the row in the database
	update Review set isApproved=_isApproved, text=_text where reviewID=_reviewID;
end; //

# This procedure edits a user in the database.  Required arguments are userID, isApproved, canPost, username, and email.
# Optional arguments are firstName and lastName.
create procedure AdminEditUser( _userID     int unsigned ,
                                _isApproved char(1)      ,
                                _username   varchar(40)  ,
                                _email      varchar(255) ,
                                _firstName  varchar(40)  ,
                                _lastName   varchar(40)  )
begin
	# edit the row in the database
	update User set isApproved=_isApproved, userName=_userName, email=_email, firstName=_firstName, 
		lastName=_lastName where userID=_userID;
end; //

# This procedure adds a new restaurant type into the database.  Required argument is name.  
create procedure AdminAddRestaurantType( _name varchar(80) )
begin
	# add the row into the database
	insert into RestaurantType (name) values (_name);
end; //

# This procedure edits a restaurant type in the database.  Required arguments are typeID and name.  
create procedure AdminEditRestaurantType( _typeID smallint unsigned ,
                                          _name   varchar(80)       )
begin
	# edit the row in the database
	update RestaurantType set name=_name where typeID=_typeID;
end; //






delimiter ;

##### basic user permissions
revoke all privileges, grant option from 'RestaurantUser'@'localhost';
grant select on Truefit.* to 'RestaurantUser'@'localhost' identified by '2Ffh@4gxM*OhcIj';
grant execute on function Truefit.GetDistanceInMiles to 'RestaurantUser'@'localhost';
grant execute on procedure Truefit.AddRestaurant to 'RestaurantUser'@'localhost';
grant execute on procedure Truefit.AddReview to 'RestaurantUser'@'localhost';
grant execute on procedure Truefit.EditReview to 'RestaurantUser'@'localhost';
grant execute on procedure Truefit.AddUser to 'RestaurantUser'@'localhost';
grant execute on procedure Truefit.DeleteReview to 'RestaurantUser'@'localhost';
grant execute on procedure Truefit.ChangePassword to 'RestaurantUser'@'localhost';

##### admin permissions
revoke all privileges, grant option from 'RestaurantAdmin'@'localhost';
grant select on Truefit.* to 'RestaurantAdmin'@'localhost' identified by 'mpO&q&t2AodvBxfk';
grant execute on procedure Truefit.AdminEditRestaurant to 'RestaurantAdmin'@'localhost';
grant execute on procedure Truefit.AdminEditReview to 'RestaurantAdmin'@'localhost';
grant execute on procedure Truefit.AdminEditUser to 'RestaurantAdmin'@'localhost';
grant execute on procedure Truefit.AdminAddRestaurantType to 'RestaurantAdmin'@'localhost';
grant execute on procedure Truefit.AdminEditRestaurantType to 'RestaurantAdmin'@'localhost';
