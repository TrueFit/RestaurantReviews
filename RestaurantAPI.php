<?php
  // need access to the database link for the rest of the functions
  $link = "";
  DatabaseLogin();

  // connect to the database function
  function DatabaseLogin()
  {
    global $link, $pathToDBCredentials;

    // include the login info for the database
    include $pathToDBCredentials;

    // try to sign in
    $link = mysqli_connect( "localhost",$username,$password, $realDB) or die("Connect Unsuccessful!".mysqli_error());
  }

  // this function runs a query into the database
  function RunQuery( $query )
  {
    global $link; 
    try 
    {
      // make sure they are connected still
      if( !mysqli_ping( $link ) ) 
      {
        mysqli_close( $link );
        DatabaseLogin();
      }

      // run the query
      $resultSet=mysqli_query($link, $query);
      if( !$resultSet )
      {
        throw new Exception( "MySQL Error" );
      }
    } 
    catch (Exception $e)
    {
      echo "Caught Exception! => " . $e->getMessage() . "\n";
    }

    // result set will be closed when page is done.
    return $resultSet;
  }

  // this function takes a JSON object and verifies that it's properties are permissible to enter into the database
  function EscapeAndVerifyJSON(&$object, $property, $testForNumeric)
  {
    global $link; 

    // if it's not set, we're already good
    if( isset($object[$property]) )
    {
      $object[$property] = mysqli_real_escape_string($link, $object[$property]);
      if( strtoupper($object[$property]) == "NULL" ||
          ($testForNumeric && !is_numeric($object[$property])) || 
          (!$testForNumeric && !is_string($object[$property])) )
      {
        unset($object[$property]);
      }
    }
  }

  

  // this function returns a JSON string listing all restaurant types in the database
  //
  // output JSON format:
  // { 
  //   "types": [
  //     {
  //       "typeID": ??, <-- always non-null
  //       "name": "??"  <-- always non-null
  //     },
  //     ...
  //   ]
  // }
  function GetRestaurantTypes()
  {
    global $link; 

    // initialize the string
    $returnObject = array("types" => array());

    // do the search
    $searchResults = RunQuery( "select typeID, name from RestaurantType order by name" );
    while( ($type = mysqli_fetch_assoc($searchResults)) != null )
    {
      // add in the information about this restaurant
      $returnObject["types"][] = array("typeID" => $type["typeID"], "name" => $type["name"]);
    }
    
    // finish the string and return it
    return json_encode($returnObject);
  }

  // this function takes a JSON string describing a new restaurant type and adds it to the database
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "name": "??"
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function AdminAddRestaurantType($jsonString)
  {
    global $link; 

    // parse the string into an object
    $typeInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($typeInfo, "name", false);

    // make sure all the non-null elements are in there
    if( isset($typeInfo["name"])  )
    {
      // make sure it doesn't already exist
      $count = mysqli_fetch_assoc( RunQuery( "select count(*) as num from RestaurantType where name='" . $typeInfo["name"] . "'" ) );
      if( $count["num"] > 0 )
      {
        return json_encode(array("feedback" => "Restaurant type already exists."));
      }

      // enter them into the database
      RunQuery("call AdminAddRestaurantType('" . $typeInfo["name"] . "')");

      $message = "success";
      return json_encode(array("feedback" => $message));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to add a new restaurant type.  See below for missing properties.<br><br>";
      $message .= (isset($typeInfo["name"]) ? "" : "name<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing restaurant type and edits it in the database
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "typeID": ??,
  //   "name": "??"
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function AdminEditRestaurantType($jsonString)
  {
    global $link; 

    // parse the string into an object
    $typeInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($typeInfo, "typeID", true);
    EscapeAndVerifyJSON($typeInfo, "name", false);

    // make sure all the non-null elements are in there
    if( isset($typeInfo["typeID"]) && isset($typeInfo["name"])  )
    {
      // make sure it doesn't already exist
      $count = mysqli_fetch_assoc( RunQuery( "select count(*) as num from RestaurantType where name='" . $typeInfo["name"] . 
                                             "' and typeID<>" . $typeInfo["typeID"] ) );
      if( $count["num"] > 0 )
      {
        return json_encode(array("feedback" => "Restaurant type already exists."));
      }

      // enter them into the database
      RunQuery("call AdminEditRestaurantType(" . $typeInfo["typeID"] . ",'" . $typeInfo["name"] . "')");

      $message = "success";
      return json_encode(array("feedback" => $message));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to edit a restaurant type.  See below for missing properties.<br><br>";
      $message .= (isset($typeInfo["typeID"]) ? "" : "typeID<br>");
      $message .= (isset($typeInfo["name"]) ? "" : "name<br>");
      return json_encode(array("feedback" => $message));
    }
  }



  // this function takes a JSON string describing a new restaurant and adds it to the database
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "name": "??",
  //   "typeID": ??,
  //   "location": {
  //     "addr1": "??",
  //     "addr2": "??",     <-- can be null
  //     "city": "??",
  //     "state": "??",
  //     "zip": "??",
  //     "latitude": ??,    <-- can be null
  //     "longitude": ??    <-- can be null
  //   },
  //   "phoneNumber": "??", <-- can be null
  //   "websiteURL": "??"   <-- can be null
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function AddRestaurant($jsonString)
  {
    global $link; 

    // parse the string into an object
    $restaurantInfo = json_decode($jsonString, true);
    if( !isset($restaurantInfo["location"]) )
    {
      $message = "You have not provided the necessary location information to add a new restaurant.";
      return json_encode(array("feedback" => $message));
    }

    // alias this
    $locationInfo = $restaurantInfo["location"];

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($restaurantInfo, "name", false);
    EscapeAndVerifyJSON($restaurantInfo, "typeID", true);
    EscapeAndVerifyJSON($locationInfo, "addr1", false);
    EscapeAndVerifyJSON($locationInfo, "addr2", false);
    EscapeAndVerifyJSON($locationInfo, "city", false);
    EscapeAndVerifyJSON($locationInfo, "state", false);
    EscapeAndVerifyJSON($locationInfo, "zip", false);
    EscapeAndVerifyJSON($locationInfo, "latitude", true);
    EscapeAndVerifyJSON($locationInfo, "longitude", true);
    EscapeAndVerifyJSON($restaurantInfo, "phoneNumber", false);
    EscapeAndVerifyJSON($restaurantInfo, "websiteURL", false);

    // make sure all the non-null elements are in there
    if( isset($restaurantInfo["name"]) && isset($restaurantInfo["typeID"]) && isset($locationInfo["addr1"]) && 
        isset($locationInfo["city"]) && isset($locationInfo["state"]) && isset($locationInfo["zip"]) )
    {
      RunQuery("call AddRestaurant('" . $restaurantInfo["name"] . "'," . $restaurantInfo["typeID"] . ",'" . $locationInfo["addr1"] . "'," . 
               (isset($locationInfo["addr2"]) ? ("'" . $locationInfo["addr2"] . "'") : "null") . ",'" .
               $locationInfo["city"] . "','" . $locationInfo["state"] . "','" . $locationInfo["zip"] . "'," . 
               (isset($locationInfo["latitude"]) ? $locationInfo["latitude"] : "null") . "," . 
               (isset($locationInfo["longitude"]) ? $locationInfo["longitude"] : "null") . "," . 
               (isset($restaurantInfo["phoneNumber"]) ? ("'" . $restaurantInfo["phoneNumber"] . "'") : "null") . "," .
               (isset($restaurantInfo["websiteURL"]) ? ("'" . $restaurantInfo["websiteURL"] . "'") : "null") . ")");

      $message = "success";
      return json_encode(array("feedback" => $message));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to add a new restaurant.  See below for missing properties.<br><br>";
      $message .= (isset($restaurantInfo["name"]) ? "" : "name<br>");
      $message .= (isset($restaurantInfo["typeID"]) ? "" : "typeID<br>");
      $message .= (isset($locationInfo["addr1"]) ? "" : "addr1<br>");
      $message .= (isset($locationInfo["city"]) ? "" : "city<br>");
      $message .= (isset($locationInfo["state"]) ? "" : "state<br>");
      $message .= (isset($locationInfo["zip"]) ? "" : "zip<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing a restaurant and updates it in the database
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "restaurantID": ??,
  //   "name": "??",
  //   "isApproved": "??",
  //   "typeID": ??,
  //   "location": {
  //     "addr1": "??",
  //     "addr2": "??",     <-- can be null
  //     "city": "??",
  //     "state": "??",
  //     "zip": "??",
  //     "latitude": ??,    <-- can be null
  //     "longitude": ??    <-- can be null
  //   },
  //   "phoneNumber": "??", <-- can be null
  //   "websiteURL": "??"   <-- can be null
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function AdminEditRestaurant($jsonString)
  {
    global $link; 

    // parse the string into an object
    $restaurantInfo = json_decode($jsonString, true);
    if( !isset($restaurantInfo["location"]) )
    {
      $message = "You have not provided the necessary location information to edit an existing restaurant.";
      return json_encode(array("feedback" => $message));
    }

    // alias this
    $locationInfo = $restaurantInfo["location"];

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($restaurantInfo, "restaurantID", true);
    EscapeAndVerifyJSON($restaurantInfo, "name", false);
    EscapeAndVerifyJSON($restaurantInfo, "isApproved", false);
    EscapeAndVerifyJSON($restaurantInfo, "typeID", true);
    EscapeAndVerifyJSON($locationInfo, "addr1", false);
    EscapeAndVerifyJSON($locationInfo, "addr2", false);
    EscapeAndVerifyJSON($locationInfo, "city", false);
    EscapeAndVerifyJSON($locationInfo, "state", false);
    EscapeAndVerifyJSON($locationInfo, "zip", false);
    EscapeAndVerifyJSON($locationInfo, "latitude", true);
    EscapeAndVerifyJSON($locationInfo, "longitude", true);
    EscapeAndVerifyJSON($restaurantInfo, "phoneNumber", false);
    EscapeAndVerifyJSON($restaurantInfo, "websiteURL", false);

    // make sure all the non-null elements are in there
    if( isset($restaurantInfo["restaurantID"]) && isset($restaurantInfo["name"]) && isset($restaurantInfo["isApproved"]) && 
        isset($restaurantInfo["typeID"]) && isset($locationInfo["addr1"]) && isset($locationInfo["city"]) && 
        isset($locationInfo["state"]) && isset($locationInfo["zip"]) )
    {
      RunQuery("call AdminEditRestaurant(" . $restaurantInfo["restaurantID"] . ",'" . $restaurantInfo["isApproved"] . "','" . 
               $restaurantInfo["name"] . "'," . $restaurantInfo["typeID"] . ",'" . $locationInfo["addr1"] . "'," . 
               (isset($locationInfo["addr2"]) ? ("'" . $locationInfo["addr2"] . "'") : "null") . ",'" .
               $locationInfo["city"] . "','" . $locationInfo["state"] . "','" . $locationInfo["zip"] . "'," . 
               (isset($locationInfo["latitude"]) ? $locationInfo["latitude"] : "null") . "," . 
               (isset($locationInfo["longitude"]) ? $locationInfo["longitude"] : "null") . "," . 
               (isset($restaurantInfo["phoneNumber"]) ? ("'" . $restaurantInfo["phoneNumber"] . "'") : "null") . "," .
               (isset($restaurantInfo["websiteURL"]) ? ("'" . $restaurantInfo["websiteURL"] . "'") : "null") . ")");

      $message = "success";
      return json_encode(array("feedback" => $message));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to edit an existing restaurant.  See below for missing properties.<br><br>";
      $message .= (isset($restaurantInfo["restaurantID"]) ? "" : "restaurantID<br>");
      $message .= (isset($restaurantInfo["name"]) ? "" : "name<br>");
      $message .= (isset($restaurantInfo["isApproved"]) ? "" : "isApproved<br>");
      $message .= (isset($restaurantInfo["typeID"]) ? "" : "typeID<br>");
      $message .= (isset($locationInfo["addr1"]) ? "" : "addr1<br>");
      $message .= (isset($locationInfo["city"]) ? "" : "city<br>");
      $message .= (isset($locationInfo["state"]) ? "" : "state<br>");
      $message .= (isset($locationInfo["zip"]) ? "" : "zip<br>");
      return json_encode(array("feedback" => $message));
    }
  }



  // this function takes a JSON string describing a new user and adds it to the database
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "username": "??",
  //   "email": "??",
  //   "pwHash": "??",
  //   "firstName": "??", <-- can be null
  //   "lastName": "??"   <-- can be null
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function AddUser($jsonString)
  {
    global $link; 

    // parse the string into an object
    $userInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($userInfo, "username", false);
    EscapeAndVerifyJSON($userInfo, "email", false);
    EscapeAndVerifyJSON($userInfo, "pwHash", false);
    EscapeAndVerifyJSON($userInfo, "firstName", false);
    EscapeAndVerifyJSON($userInfo, "lastName", false);

    // make sure all the non-null elements are in there
    if( isset($userInfo["username"]) && isset($userInfo["email"]) && isset($userInfo["pwHash"]) )
    {
      // make sure that username and email are not in use
      $usernameResults = mysqli_fetch_assoc( RunQuery( "select count(*) as num from User where username='" . $userInfo["username"] . "'") );
      $emailResults = mysqli_fetch_assoc( RunQuery( "select count(*) as num from User where email='" . $userInfo["email"] . "'") );
      if( $usernameResults["num"] > 0 )
      {
        $message = "Username already in use.";
        return json_encode(array("feedback" => $message));
      }
      else if( $emailResults["num"] > 0 )
      {
        $message = "Email address already in use.";
        return json_encode(array("feedback" => $message));
      }

      // generate the salt
      $salt = "";
      $allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      for( $i=0; $i<64; $i++ )
      {
        $salt .= substr($allowedChars, mt_rand(0, strlen($allowedChars) - 1), 1);
      }

      // enter them into the database
      RunQuery("call AddUser('" . $userInfo["username"] . "','" . $userInfo["email"] . "','" . $salt . 
               "',unhex(sha2(concat('" . $salt . "', upper('" . $userInfo["pwHash"] . "')), 256))," . 
               (isset($userInfo["firstName"]) ? ("'" . $userInfo["firstName"] . "'") : "null") . "," .
               (isset($userInfo["lastName"]) ? ("'" . $userInfo["lastName"] . "'") : "null") . ")");

      $message = "success";
      return json_encode(array("feedback" => $message));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to add a new user.  See below for missing properties.<br><br>";
      $message .= (isset($userInfo["username"]) ? "" : "username<br>");
      $message .= (isset($userInfo["email"]) ? "" : "email<br>");
      $message .= (isset($userInfo["pwHash"]) ? "" : "pwHash<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing an existing user and updates it in the database
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "userID": ??,
  //   "isApproved": "??",
  //   "username": "??",
  //   "email": "??",
  //   "pwHash": "??",
  //   "firstName": "??", <-- can be null
  //   "lastName": "??"   <-- can be null
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function AdminEditUser($jsonString)
  {
    global $link; 

    // parse the string into an object
    $userInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($userInfo, "userID", true);
    EscapeAndVerifyJSON($userInfo, "isApproved", false);
    EscapeAndVerifyJSON($userInfo, "username", false);
    EscapeAndVerifyJSON($userInfo, "email", false);
    EscapeAndVerifyJSON($userInfo, "pwHash", false);
    EscapeAndVerifyJSON($userInfo, "firstName", false);
    EscapeAndVerifyJSON($userInfo, "lastName", false);

    // make sure all the non-null elements are in there
    if( isset($userInfo["userID"]) && isset($userInfo["isApproved"]) && isset($userInfo["username"]) && 
        isset($userInfo["email"]) && isset($userInfo["pwHash"]) )
    {
      // make sure that username and email are not in use
      $usernameResults = mysqli_fetch_assoc( RunQuery( "select count(*) as num from User where username='" . $userInfo["username"] . 
                                                       "' and userID != " . $userInfo["userID"]) );
      $emailResults = mysqli_fetch_assoc( RunQuery( "select count(*) as num from User where email='" . $userInfo["email"] . 
                                                    "' and userID != " . $userInfo["userID"]) );
      if( $usernameResults["num"] > 0 )
      {
        $message = "Username already in use.";
        return json_encode(array("feedback" => $message));
      }
      else if( $emailResults["num"] > 0 )
      {
        $message = "Email address already in use.";
        return json_encode(array("feedback" => $message));
      }

      // enter them into the database
      RunQuery("call AdminEditUser(" . $userInfo["userID"] . ",'" . $userInfo["isApproved"] . "','" . $userInfo["username"] . 
               "','" . $userInfo["email"] . "'," . 
               (isset($userInfo["firstName"]) ? ("'" . $userInfo["firstName"] . "'") : "null") . "," .
               (isset($userInfo["lastName"]) ? ("'" . $userInfo["lastName"] . "'") : "null") . ")");

      $message = "success";
      return json_encode(array("feedback" => $message));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to edit an existing user.  See below for missing properties.<br><br>";
      $message .= (isset($userInfo["userID"]) ? "" : "userID<br>");
      $message .= (isset($userInfo["isApproved"]) ? "" : "isApproved<br>");
      $message .= (isset($userInfo["username"]) ? "" : "username<br>");
      $message .= (isset($userInfo["email"]) ? "" : "email<br>");
      $message .= (isset($userInfo["pwHash"]) ? "" : "pwHash<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing a user and compares it to the database to see whether
  // they are a proper user
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "username": "??",
  //   "pwHash": "??"
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function VerifyUser($jsonString)
  {
    global $link; 

    // parse the string into an object
    $userInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($userInfo, "username", false);
    EscapeAndVerifyJSON($userInfo, "pwHash", false);

    // make sure all the non-null elements are in there
    if( isset($userInfo["username"]) && isset($userInfo["pwHash"]) )
    {
      // try to log them in
      $userResults = mysqli_fetch_assoc( RunQuery( "select count(*) as num from User where username='" . $userInfo["username"] . "'" ) );
      $loginResults = mysqli_fetch_assoc( RunQuery( "select upper(sha2(concat(salt, upper('" . $userInfo["pwHash"] . "')), 256)) as pwTry, " .
                                                    "hex(passwordHash) as pwMatch from User where username='" . $userInfo["username"] . "'" ) );
      // see if that user exists
      if( $userResults["num"] == 0 )
      {
        $message = "No user by that name.";
        return json_encode(array("feedback" => $message));
      }
      // see if they know the password
      else if( $loginResults["pwTry"] != $loginResults["pwMatch"] )
      {
        $message = "Incorrect password.";
        return json_encode(array("feedback" => $message));
      }

      // that's the right password, let them know
      $message = "success";
      return json_encode(array("feedback" => $message));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to log a user in.  See below for missing properties.<br><br>";
      $message .= (isset($userInfo["username"]) ? "" : "username<br>");
      $message .= (isset($userInfo["pwHash"]) ? "" : "pwHash<br>");
      return json_encode(array("feedback" => $message));
    }
  }



  // this function takes a JSON string describing an ID and returns the matching restaurant
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "restaurantID": ??,
  //   "isAdmin": "??"     <- if this is present and equal to YES, we can override the isApproved flags
  // }
  //
  // output JSON format: (returns "success" for the feedback if all went well, otherwise the error)
  //                     (see the function GetRestaurantArrayFromQuery below for the details of the restaurant section)
  // { 
  //   "feedback": "??", <-- always non-null
  //   "restaurant": {}
  // }
  //
  function GetRestaurantByID($jsonString)
  {
    global $link; 

    // parse the string into an object
    $restaurantInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($restaurantInfo, "restaurantID", true);
    EscapeAndVerifyJSON($restaurantInfo, "isAdmin", false);

    // make sure all the non-null elements are in there
    if( isset($restaurantInfo["restaurantID"]) )
    {
      $admin = isset($restaurantInfo["isAdmin"]) && ($restaurantInfo["isAdmin"] == "YES");
      $query = "select restaurantID, Restaurant.name as name, isApproved, typeID, RestaurantType.name as typeName, " . 
               "address1, address2, city, state, zip, latitude, longitude, null as distance, phoneNumber, websiteURL " . 
               "from Restaurant join RestaurantType using (typeID) where restaurantID=" . $restaurantInfo["restaurantID"] . 
               ($admin ? "" : " and isApproved in ('Y','U')");

      // call the helper function that formats it properly
      $restaurantList = GetRestaurantArrayFromQuery($query);
      return json_encode(array("feedback" => "success", "restaurant" => $restaurantList[0]));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to search by ID.  See below for missing properties.<br><br>";
      $message .= (isset($restaurantInfo["restaurantID"]) ? "" : "restaurantID<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing a city and returns the restaurants located there
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "city": "??",
  //   "state": "??",
  //   "isAdmin": "??"     <- if this is present and equal to YES, we can override the isApproved flags
  // }
  //
  // output JSON format: (returns "success" for the feedback if all went well, otherwise the error)
  //                     (see the function GetRestaurantArrayFromQuery below for the details of the restaurants section)
  // { 
  //   "feedback": "??", <-- always non-null
  //   "restaurants": []
  // }
  //
  function GetRestaurantsByCity($jsonString)
  {
    global $link; 

    // parse the string into an object
    $cityInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($cityInfo, "city", false);
    EscapeAndVerifyJSON($cityInfo, "state", false);
    EscapeAndVerifyJSON($cityInfo, "isAdmin", false);

    // make sure all the non-null elements are in there
    if( isset($cityInfo["city"]) && isset($cityInfo["state"]) )
    {
      $admin = isset($cityInfo["isAdmin"]) && ($cityInfo["isAdmin"] == "YES");
      $query = "select restaurantID, Restaurant.name as name, isApproved, typeID, RestaurantType.name as typeName, " . 
               "address1, address2, city, state, zip, latitude, longitude, null as distance, phoneNumber, websiteURL " . 
               "from Restaurant join RestaurantType using (typeID) where upper(city)=upper('" . $cityInfo["city"] . 
               "') and upper(state)=upper('" . $cityInfo["state"] . "') " . ($admin ? "" : "and isApproved in ('Y', 'U') ") . 
               "order by Restaurant.name";

      // call the helper function that formats it properly
      $restaurantList = GetRestaurantArrayFromQuery($query);
      return json_encode(array("feedback" => "success", "restaurants" => $restaurantList));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to search by city.  See below for missing properties.<br><br>";
      $message .= (isset($cityInfo["city"]) ? "" : "city<br>");
      $message .= (isset($cityInfo["state"]) ? "" : "state<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing a restaurant name and returns any matches we find
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "name": "??",
  //   "isAdmin": "??"     <- if this is present and equal to YES, we can override the isApproved flags
  // }
  //
  // output JSON format: (returns "success" for the feedback if all went well, otherwise the error)
  //                     (see the function GetRestaurantArrayFromQuery below for the details of the restaurants section)
  // { 
  //   "feedback": "??", <-- always non-null
  //   "restaurants": []
  // }
  //
  function GetRestaurantsByName($jsonString)
  {
    global $link; 

    // parse the string into an object
    $nameInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($nameInfo, "name", false);
    EscapeAndVerifyJSON($nameInfo, "isAdmin", false);

    // make sure all the non-null elements are in there
    if( isset($nameInfo["name"]) )
    {
      $admin = isset($nameInfo["isAdmin"]) && ($nameInfo["isAdmin"] == "YES");
      $query = "select restaurantID, Restaurant.name as name, isApproved, typeID, RestaurantType.name as typeName, " . 
               "address1, address2, city, state, zip, latitude, longitude, null as distance, phoneNumber, websiteURL " . 
               "from Restaurant join RestaurantType using (typeID) where upper(Restaurant.name) like concat('%', upper('" . 
               $nameInfo["name"] . "'), '%') " .  ($admin ? "" : "and isApproved in ('Y', 'U') ") . "order by Restaurant.name";

      // call the helper function that formats it properly
      $restaurantList = GetRestaurantArrayFromQuery($query);
      return json_encode(array("feedback" => "success", "restaurants" => $restaurantList));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to search by name.  See below for missing properties.<br><br>";
      $message .= (isset($nameInfo["name"]) ? "" : "name<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing a location and distance in miles and returns any 
  // restaurants within that range.
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "latitude": ??,
  //   "longitude": ??,
  //   "distanceInMiles": ??,
  //   "isAdmin": "??"        <- if this is present and equal to YES, we can override the isApproved flags
  // }
  //
  // output JSON format: (returns "success" for the feedback if all went well, otherwise the error)
  //                     (see the function GetRestaurantArrayFromQuery below for the details of the restaurants section)
  // { 
  //   "feedback": "??", <-- always non-null
  //   "restaurants": []
  // }
  //
  function GetRestaurantsByDistance($jsonString)
  {
    global $link; 

    // parse the string into an object
    $locationInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($locationInfo, "latitude", true);
    EscapeAndVerifyJSON($locationInfo, "longitude", true);
    EscapeAndVerifyJSON($locationInfo, "distanceInMiles", true);
    EscapeAndVerifyJSON($locationInfo, "isAdmin", true);

    // make sure all the non-null elements are in there
    if( isset($locationInfo["latitude"]) && isset($locationInfo["longitude"]) && isset($locationInfo["distanceInMiles"]) )
    {
      $admin = isset($locationInfo["isAdmin"]) && ($locationInfo["isAdmin"] == "YES");
      $query = "select restaurantID, Restaurant.name as name, isApproved, typeID, RestaurantType.name as typeName, " . 
               "address1, address2, city, state, zip, latitude, longitude, GetDistanceInMiles(" . $locationInfo["latitude"] . 
               "," . $locationInfo["longitude"] . ",latitude, longitude) as distance, phoneNumber, websiteURL " . 
               "from Restaurant join RestaurantType using (typeID) where latitude is not null and longitude is not null " . 
               ($admin ? "" : "and isApproved in ('Y', 'U') ") . "having distance <= " . $locationInfo["distanceInMiles"] . 
               " order by distance asc";

      // call the helper function that formats it properly
      $restaurantList = GetRestaurantArrayFromQuery($query);
      return json_encode(array("feedback" => "success", "restaurants" => $restaurantList));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to search by distance.  See below for missing properties.<br><br>";
      $message .= (isset($locationInfo["latitude"]) ? "" : "latitude<br>");
      $message .= (isset($locationInfo["longitude"]) ? "" : "longitude<br>");
      $message .= (isset($locationInfo["distanceInMiles"]) ? "" : "distanceInMiles<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this is a helper function for the rest of the API.  it takes as an argument a SQL string that will return a
  // series of restaurants and the associated data.  it returns a PHP array containing all of the information,
  // so every function that outputs restaurant data should be leveraging this and returning the data structure below.
  //
  // output JSON format:
  // [
  //   {
  //     "ID": ??,             <-- always non-null
  //     "name": "??",         <-- always non-null
  //     "isApproved": "??",   <-- always non-null
  //     "typeID": ??,         <-- always non-null
  //     "typeName": "??",     <-- always non-null
  //     "location": {         <-- always non-null
  //       "addr1": "??",      <-- always non-null
  //       "addr2": "??",
  //       "city": "??",       <-- always non-null
  //       "state": "??",      <-- always non-null
  //       "zip": "??",        <-- always non-null
  //       "latitude": ??,
  //       "longitude": ??,
  //       "distanceInMiles": ??
  //     },
  //     "phoneNumber": "??",
  //     "websiteURL": "??",
  //     "reviews": [          <-- always non-null
  //       {
  //         "reviewID": ??,   <-- always non-null
  //         "userID": ??,     <-- always non-null
  //         "userName": "??", <-- always non-null
  //         "postTime": "??", <-- always non-null
  //         "rating": ??,
  //         "price": ??,
  //         "text": "??"      <-- always non-null
  //       },
  //       ...
  //     ],
  //     "rating": {           <-- always non-null
  //       "count": ??,        <-- always non-null
  //       "total": ??         <-- always non-null
  //     },
  //     "price": {            <-- always non-null
  //       "count": ??,        <-- always non-null
  //       "total": ??         <-- always non-null
  //     }
  //   },
  //   ...
  // ]
  function GetRestaurantArrayFromQuery($query)
  {
    // initialize the array
    $restaurants = array();

    // do the search
    $searchResults = RunQuery( $query );
    while( ($row = mysqli_fetch_assoc($searchResults)) != null )
    {
      // build the location object
      $thisLocation = array("addr1" => $row["address1"], "addr2" => $row["address2"], "city" => $row["city"],
                            "state" => $row["state"], "zip" => $row["zip"], "latitude" => $row["latitude"],
                            "longitude" => $row["longitude"], "distanceInMiles" => $row["distance"]);

      // build the reviews object
      $theseReviews = array();
      $rating = array("count" => 0, "total" => 0);
      $price = array("count" => 0, "total" => 0);
      $reviewResults = RunQuery( "select reviewID, userID, username, postTime, rating, price, reviewText from Review " . 
                                 "join User using (userID) where restaurantID = " . $row["restaurantID"] . 
                                 " and Review.isApproved in ('Y','U') and User.isApproved in ('Y','U')" );
      while( ($row2 = mysqli_fetch_assoc( $reviewResults )) != null )
      {
        // add in the needed parts of this review
        $thisReview = array("reviewID" => $row2["reviewID"], "userID" => $row2["userID"], "username" => $row2["username"],
                            "postTime" => $row2["postTime"], "rating" => $row2["rating"], "price" => $row2["price"], 
                            "text" => $row2["reviewText"]);

        // push it into the array
        $theseReviews[] = $thisReview;

        // increment these counts if needed
        if( isset($thisReview["rating"]) )
        {
          $rating["count"]++;
          $rating["total"] += $thisReview["rating"];
        }
        if( isset($thisReview["price"]) )
        {
          $price["count"]++;
          $price["total"] += $thisReview["price"];
        }
      }

      // add in the information about this restaurant
      $restaurants[] = array("ID" => $row["restaurantID"], "name" => $row["name"], "isApproved" => $row["isApproved"],
                             "typeID" => $row["typeID"], "typeName" => $row["typeName"], "location" => $thisLocation, 
                             "phoneNumber" => $row["phoneNumber"], "websiteURL" => $row["websiteURL"], "reviews" => $theseReviews, 
                             "rating" => $rating, "price" => $price);
    }

    // return it
    return $restaurants;
  }



  // this function takes a JSON string describing some login info and a review that user is trying to post.
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "loginInfo": {
  //     "username": "??",
  //     "pwHash": "??"
  //   },
  //   "restaurantID": ??,
  //   "rating": ??,       <-- can be null
  //   "price": ??,        <-- can be null
  //   "text": "??"
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function PostReview($jsonString)
  {
    global $link; 

    // parse the string into an object
    $reviewInfo = json_decode($jsonString, true);

    // make sure they sent login info
    if( !isset($reviewInfo["loginInfo"]) )
    {
      return json_encode(array("feedback" => "You have not provided any login info."));
    }

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($reviewInfo["loginInfo"], "username", false);
    EscapeAndVerifyJSON($reviewInfo["loginInfo"], "pwHash", false);
    EscapeAndVerifyJSON($reviewInfo, "restaurantID", true);
    EscapeAndVerifyJSON($reviewInfo, "rating", true);
    EscapeAndVerifyJSON($reviewInfo, "price", true);
    EscapeAndVerifyJSON($reviewInfo, "text", false);

    // make sure all the non-null elements are in there
    if( isset($reviewInfo["loginInfo"]["username"]) && isset($reviewInfo["loginInfo"]["pwHash"]) && isset($reviewInfo["restaurantID"]) &&
        isset($reviewInfo["text"]) )
    {
      // grab the userID associated with the given username
      $userResults = mysqli_fetch_assoc( RunQuery( "select userID from User where username='" . $reviewInfo["loginInfo"]["username"] . "'" ) );

      // try to post the review
      RunQuery("call AddReview(" . $userResults["userID"] . ",unhex('" . $reviewInfo["loginInfo"]["pwHash"] . "')," . 
               $reviewInfo["restaurantID"] . ",'" . $reviewInfo["text"] . "'," . 
               (isset($reviewInfo["rating"]) ? $reviewInfo["rating"] : "null") . "," . 
               (isset($reviewInfo["price"]) ? $reviewInfo["price"] : "null") . ")");

      // let them know it worked
      return json_encode(array("feedback" => "success"));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to post a review.  See below for missing properties.<br><br>";
      $message .= (isset($reviewInfo["loginInfo"]["username"]) ? "" : "username<br>");
      $message .= (isset($reviewInfo["loginInfo"]["pwHash"]) ? "" : "pwHash<br>");
      $message .= (isset($reviewInfo["restaurantID"]) ? "" : "restaurantID<br>");
      $message .= (isset($reviewInfo["text"]) ? "" : "text<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing some login info and a review that user is trying to edit.
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "loginInfo": {
  //     "username": "??",
  //     "pwHash": "??"
  //   },
  //   "reviewID": ??,
  //   "rating": ??,       <-- can be null
  //   "price": ??,        <-- can be null
  //   "text": "??"
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function EditReview($jsonString)
  {
    global $link; 

    // parse the string into an object
    $reviewInfo = json_decode($jsonString, true);

    // make sure they sent login info
    if( !isset($reviewInfo["loginInfo"]) )
    {
      return json_encode(array("feedback" => "You have not provided any login info."));
    }

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($reviewInfo["loginInfo"], "username", false);
    EscapeAndVerifyJSON($reviewInfo["loginInfo"], "pwHash", false);
    EscapeAndVerifyJSON($reviewInfo, "reviewID", true);
    EscapeAndVerifyJSON($reviewInfo, "rating", true);
    EscapeAndVerifyJSON($reviewInfo, "price", true);
    EscapeAndVerifyJSON($reviewInfo, "text", false);

    // make sure all the non-null elements are in there
    if( isset($reviewInfo["loginInfo"]["username"]) && isset($reviewInfo["loginInfo"]["pwHash"]) && isset($reviewInfo["reviewID"]) &&
        isset($reviewInfo["text"]) )
    {
      // grab the userID associated with the given username
      $userResults = mysqli_fetch_assoc( RunQuery( "select userID from User where username='" . $reviewInfo["loginInfo"]["username"] . "'" ) );

      // try to edit the review
      RunQuery("call EditReview(" . $userResults["userID"] . ",unhex('" . $reviewInfo["loginInfo"]["pwHash"] . "')," . 
               $reviewInfo["reviewID"] . ",'" . $reviewInfo["text"] . "'," . 
               (isset($reviewInfo["rating"]) ? $reviewInfo["rating"] : "null") . "," . 
               (isset($reviewInfo["price"]) ? $reviewInfo["price"] : "null") . ")");

      // let them know it worked
      return json_encode(array("feedback" => "success"));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to edit a review.  See below for missing properties.<br><br>";
      $message .= (isset($reviewInfo["loginInfo"]["username"]) ? "" : "username<br>");
      $message .= (isset($reviewInfo["loginInfo"]["pwHash"]) ? "" : "pwHash<br>");
      $message .= (isset($reviewInfo["reviewID"]) ? "" : "reviewID<br>");
      $message .= (isset($reviewInfo["text"]) ? "" : "text<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing a review that the admin is trying to edit.
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "reviewID": ??,
  //   "isApproved"; "??",
  //   "text": "??"
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function AdminEditReview($jsonString)
  {
    global $link; 

    // parse the string into an object
    $reviewInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($reviewInfo, "reviewID", true);
    EscapeAndVerifyJSON($reviewInfo, "isApproved", false);
    EscapeAndVerifyJSON($reviewInfo, "text", false);

    // make sure all the non-null elements are in there
    if( isset($reviewInfo["reviewID"]) && isset($reviewInfo["isApproved"]) && isset($reviewInfo["text"]) )
    {
      // try to edit the review
      RunQuery("call AdminEditReview(" . $reviewInfo["reviewID"] . ",'" . $reviewInfo["isApproved"] . "','" . $reviewInfo["text"] . "')");

      // let them know it worked
      return json_encode(array("feedback" => "success"));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to edit a review.  See below for missing properties.<br><br>";
      $message .= (isset($reviewInfo["reviewID"]) ? "" : "reviewID<br>");
      $message .= (isset($reviewInfo["isApproved"]) ? "" : "isApproved<br>");
      $message .= (isset($reviewInfo["text"]) ? "" : "text<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing some login info and a review that user is trying to delete.
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "loginInfo": {
  //     "username": "??",
  //     "pwHash": "??"
  //   },
  //   "reviewID": ??
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??" <-- always non-null
  // }
  function DeleteReview($jsonString)
  {
    global $link; 

    // parse the string into an object
    $reviewInfo = json_decode($jsonString, true);

    // make sure they sent login info
    if( !isset($reviewInfo["loginInfo"]) )
    {
      return json_encode(array("feedback" => "You have not provided any login info."));
    }

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($reviewInfo["loginInfo"], "username", false);
    EscapeAndVerifyJSON($reviewInfo["loginInfo"], "pwHash", false);
    EscapeAndVerifyJSON($reviewInfo, "reviewID", true);

    // make sure all the non-null elements are in there
    if( isset($reviewInfo["loginInfo"]["username"]) && isset($reviewInfo["loginInfo"]["pwHash"]) && isset($reviewInfo["reviewID"]) )
    {
      // grab the userID associated with the given username
      $userResults = mysqli_fetch_assoc( RunQuery( "select userID from User where username='" . $reviewInfo["loginInfo"]["username"] . "'" ) );

      // try to edit the review
      RunQuery("call DeleteReview(" . $userResults["userID"] . ",unhex('" . $reviewInfo["loginInfo"]["pwHash"] . "')," . 
               $reviewInfo["reviewID"] . ")");

      // let them know it worked
      return json_encode(array("feedback" => "success"));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to delete a review.  See below for missing properties.<br><br>";
      $message .= (isset($reviewInfo["loginInfo"]["username"]) ? "" : "username<br>");
      $message .= (isset($reviewInfo["loginInfo"]["pwHash"]) ? "" : "pwHash<br>");
      $message .= (isset($reviewInfo["reviewID"]) ? "" : "reviewID<br>");
      return json_encode(array("feedback" => $message));
    }
  }



  // this function takes a JSON string describing a user name and returns any matching users who have written reviews
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "username": "??",
  //   "isAdmin": "??"     <- if this is present and equal to YES, we can override the isApproved flags
  // }
  //
  // output JSON format: (returns "success" for the feedback if all went well, otherwise the error)
  // { 
  //   "feedback": "??",     <-- always non-null
  //   "users": [            <-- always non-null
  //     {
  //       "userID": ??,     <-- always non-null
  //       "username": "??", <-- always non-null
  //       "reviewCount": ?? <-- always non-null
  //     },
  //     ...
  //   ]
  // }
  //
  function GetUsersByName($jsonString)
  {
    global $link; 

    // parse the string into an object
    $nameInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($nameInfo, "username", false);
    EscapeAndVerifyJSON($nameInfo, "isAdmin", false);

    // make sure all the non-null elements are in there
    if( isset($nameInfo["username"]) )
    {
      $admin = (isset($nameInfo["isAdmin"]) && ($nameInfo["isAdmin"] == "YES"));
      $results = RunQuery( "select userID, username, count(distinct reviewID) as reviewCount from User join Review using (userID) " . 
                           "join Restaurant using (restaurantID) where upper(username) " . 
                           "like concat('%', upper('" . $nameInfo["username"] . "'), '%') " . 
                           ($admin ? "" 
                                   : ("and User.isApproved in ('Y', 'U') and Review.isApproved in ('Y', 'U') " . 
                                      "and Restaurant.isApproved in ('Y', 'U') ")) .
                           "order by username" );

      // run through the list
      $userList = array();
      while( ($row = mysqli_fetch_assoc( $results )) != null && $row["userID"] != "0" )
      {
        $userList[] = array("userID" => $row["userID"], "username" => $row["username"], "reviewCount" => $row["reviewCount"]);
      }

      // send it back
      return json_encode(array("feedback" => "success", "users" => $userList));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to search by name.  See below for missing properties.<br><br>";
      $message .= (isset($nameInfo["username"]) ? "" : "username<br>");
      return json_encode(array("feedback" => $message));
    }
  }

  // this function takes a JSON string describing a user and returns all reviews they have written.
  //
  // input JSON format (fill in the ??s as needed):
  // { 
  //   "userID": ??,
  //   "isAdmin": "??"     <- if this is present and equal to YES, we can override the isApproved flags
  // }
  //
  // output JSON format (returns "success" if all went well, otherwise the error)
  // {
  //   "feedback": "??",             <-- always non-null
  //   "userID": ??,                 <-- always non-null
  //   "userName": "??",             <-- always non-null
  //   "reviews": [                  <-- always non-null
  //     {
  //         "reviewID": ??,         <-- always non-null
  //         "restaurant": {         <-- always non-null
  //           "restaurantID": ??,   <-- always non-null
  //           "name": "??",         <-- always non-null
  //           "typeID": ??,         <-- always non-null
  //           "typeName": "??",     <-- always non-null
  //           "location": {         <-- always non-null
  //             "addr1": "??",      <-- always non-null
  //             "addr2": "??",
  //             "city": "??",       <-- always non-null
  //             "state": "??",      <-- always non-null
  //             "zip": "??",        <-- always non-null
  //             "latitude": ??,
  //             "longitude": ??,
  //             "distanceInMiles": ??
  //           },
  //           "phoneNumber": "??",
  //           "websiteURL": "??",
  //         },
  //         "postTime": "??",       <-- always non-null
  //         "rating": ??,
  //         "price": ??,
  //         "text": "??"            <-- always non-null
  //     },
  //     ...
  //   ]
  // }
  function GetReviewsByUserID($jsonString)
  {
    global $link; 

    // parse the string into an object
    $userInfo = json_decode($jsonString, true);

    // escape all of the members and verify they are what they claim to be
    EscapeAndVerifyJSON($userInfo, "userID", true);
    EscapeAndVerifyJSON($userInfo, "isAdmin", true);

    // make sure all the non-null elements are in there
    if( isset($userInfo["userID"]) )
    {
      // grab that person's username
      $username = mysqli_fetch_assoc( RunQuery( "select coalesce(username, 'No user found') as name from User where userID=" . 
                                                $userInfo["userID"] ) );
      $userInfo["username"] = $username["name"];

      // try to find the reviews
      $admin = (isset($userInfo["isAdmin"]) && ($userInfo["isAdmin"] == "YES"));
      $theseReviews = array();
      $reviewResults = RunQuery( "select reviewID, restaurantID, Restaurant.name as rName, typeID, " . 
                                 "RestaurantType.name as tName, address1, address2, city, state, zip, latitude, longitude, " . 
                                 "null as distance, phoneNumber, websiteURL, postTime, rating, price, reviewText " . 
                                 "from Review join User using (userID) join Restaurant using (restaurantID) " . 
                                 "join RestaurantType using (typeID) where userID = " . $userInfo["userID"] . 
                                 ($admin ? "" : (" and Review.isApproved in ('Y','U') and User.isApproved in ('Y','U') " . 
                                                 "and Restaurant.isApproved in ('Y', 'U')")) );
      while( ($row = mysqli_fetch_assoc( $reviewResults )) != null )
      {
        // build the location object
        $location = array("addr1" => $row["address1"], "addr2" => $row["address2"], "city" => $row["city"], 
                          "state" => $row["state"], "zip" => $row["zip"], "latitude" => $row["latitude"], 
                          "longitude" => $row["longitude"], "distanceInMiles" => $row["distance"] );

        // build the restaurant object
        $thisRestaurant = array("restaurantID" => $row["restaurantID"], "name" => $row["rName"], "typeID" => $row["typeID"], 
                                "typeName" => $row["tName"], "location" => $location, "phoneNumber" => $row["phoneNumber"], 
                                "websiteURL" => $row["websiteURL"] );

        // add in the needed parts of this review
        $thisReview = array("reviewID" => $row["reviewID"], "restaurant" => $thisRestaurant, "postTime" => $row["postTime"], 
                            "rating" => $row["rating"], "price" => $row["price"], "text" => $row["reviewText"]);

        // push it into the array
        $theseReviews[] = $thisReview;
      }

      // let them know it worked
      return json_encode(array("feedback" => "success", "userID" => $userInfo["userID"], 
                               "username" => $userInfo["username"], "reviews" => $theseReviews));
    }
    // let them know they didn't provide the necessary information
    else
    {
      $message = "You have not provided all of the required fields to get reviews.  See below for missing properties.<br><br>";
      $message .= (isset($userInfo["userID"]) ? "" : "userID<br>");
      return json_encode(array("feedback" => $message));
    }
  }
?>
