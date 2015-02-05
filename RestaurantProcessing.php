<?php
  session_start();

  // see whether they logged out
  if( isset($_POST["logout"]) && $_POST["logout"] == "true" )
  {
    unset($_SESSION["username"]);
    unset($_SESSION["pwHash"]);
    setcookie("username", "", 1, "/", $_SERVER["SERVER_NAME"]);
    setcookie("pwHash", "", 1, "/", $_SERVER["SERVER_NAME"]);
    unset($_COOKIE["username"]);
    unset($_COOKIE["pwHash"]);
  }

  // some cookie stuff to keep them logged in
  function CheckCookie($property)
  {
    // they have a cookie and no session
    if( isset($_COOKIE[$property]) && !isset($_SESSION[$property]) )
    {
      $_SESSION[$property] = $_COOKIE[$property];
      // extend the cookie
      setcookie($property, $_SESSION[$property], time() + 3600 * 24 * 30, "/", $_SERVER["SERVER_NAME"]);
    }
  }
  CheckCookie("username");
  CheckCookie("pwHash");

  // check to see whether they tried to create an account
  $createError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "createUser" )
  {
    // make sure they provided all of the required fields
    if( !isset($_POST["username"]) || $_POST["username"] == "" )
    {
      $createError = "You must enter a username.";
    }
    else if( !isset($_POST["password"]) || $_POST["password"] == "" )
    {
      $createError = "You must enter a password.";
    }
    else if( !isset($_POST["confirmPW"]) || $_POST["confirmPW"] == "" )
    {
      $createError = "You must confirm your password.";
    }
    else if( !isset($_POST["email"]) || $_POST["email"] == "" )
    {
      $createError = "You must enter an email address.";
    }
    // see whether the passwords match
    else if( $_POST["password"] != $_POST["confirmPW"] )
    {
      $createError = "Passwords do not match.";
    }
    // see whether the email address is invalid
    else if( substr_count($_POST["email"], "@") != 1 )
    {
      $createError = "Email address invalid.";
    }
    // all of the preliminary tests passed.  send it to the API.
    else
    {
      $JSON = array("username" => $_POST["username"], "email" => $_POST["email"], 
                    "pwHash" => hash('sha256', $_POST["password"]), 
                    "firstName" => $_POST["firstName"], "lastName" => $_POST["lastName"]);

      // call the API function
      $addUserOutput = json_decode(AddUser(json_encode($JSON)), true);
      if( $addUserOutput["feedback"] == "success" )
      {
        $createError = "SignThemIn";
      }
      else
      {
        $createError = $addUserOutput["feedback"];
      }
    }
  }

  // check to see whether they tried to sign in
  $loginError = "&nbsp;";
  if( (isset($_POST["task"]) && $_POST["task"] == "login") || $createError == "SignThemIn" )
  {
    $createError = "&nbsp;";
    // make sure they provided all of the required fields
    if( !isset($username) && (!isset($_POST["loginUsername"]) || $_POST["loginUsername"] == "") )
    {
      $loginError = "You must enter a username.";
    }
    else if( !isset($password) && (!isset($_POST["loginPassword"]) || $_POST["loginPassword"] == "") )
    {
      $loginError = "You must enter a password.";
    }
    // they must be ok.  send it the to API.
    else
    {
      $JSON = array("username" => $_POST["loginUsername"], "pwHash" => hash('sha256', $_POST["loginPassword"]));

      // call the API function
      $verifyUserOutput = json_decode(VerifyUser(json_encode($JSON)), true);
      if( $verifyUserOutput["feedback"] == "success" )
      {
        $loginError = "&nbsp;";

        setcookie("username", $JSON["username"], time() + 3600 * 24 * 30, "/", $_SERVER["SERVER_NAME"]);
        setcookie("pwHash", $JSON["pwHash"], time() + 3600 * 24 * 30, "/", $_SERVER["SERVER_NAME"]);
        $_SESSION["username"] = $JSON["username"];
        $_SESSION["pwHash"] = $JSON["pwHash"];
      }
      else
      {
        $loginError = $verifyUserOutput["feedback"];
      }
    }
  }

  // check to see whether they tried to add a new restaurant
  $addError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "addRestaurant" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["addName"]) || $_POST["addName"] == "" )
    {
      $addError = "You must enter a restaurant name.";
    }
    else if( !isset($_POST["addType"]) || $_POST["addType"] == "" )
    {
      $addError = "You must enter a restaurant type.";
    }
    else if( !isset($_POST["addAddr1"]) || $_POST["addAddr1"] == "" )
    {
      $addError = "You must enter an address.";
    }
    else if( !isset($_POST["addCity"]) || $_POST["addCity"] == "" )
    {
      $addError = "You must enter a city.";
    }
    else if( !isset($_POST["addState"]) || $_POST["addState"] == "" )
    {
      $addError = "You must enter a state.";
    }
    else if( !isset($_POST["addZip"]) || $_POST["addZip"] == "" )
    {
      $addError = "You must enter a zip code.";
    }
    // they must be ok.  send it to the API
    else
    {
      // convert the coordinates to absolute
      $latitude = ($_POST["addLatitude"] == "" || $_POST["addLatitudeDirection"] == "") 
                   ? "" : ($_POST["addLatitude"] * $_POST["addLatitudeDirection"]);
      $longitude = ($_POST["addLongitude"] == "" || $_POST["addLongitudeDirection"] == "") 
                    ? "" : ($_POST["addLongitude"] * $_POST["addLongitudeDirection"]);

      // build the JSON objects
      $location = array("addr1" => $_POST["addAddr1"], "addr2" => $_POST["addAddr2"], "city" => $_POST["addCity"], 
                        "state" => $_POST["addState"], "zip" => $_POST["addZip"], "latitude" => $latitude, "longitude" => $longitude);
      $JSON = array("name" => $_POST["addName"], "typeID" => $_POST["addType"], "location" => $location,
                    "phoneNumber" => $_POST["addPhone"], "websiteURL" => $_POST["addWebsite"]);

      // call the API function
      $addRestaurantOutput = json_decode(AddRestaurant(json_encode($JSON)), true);
      if( $addRestaurantOutput["feedback"] == "success" )
      {
        $addError = "New restaurant added!";
      }
      else
      {
        $addError = $addRestaurantOutput["feedback"];
      }
    }
  }

  // see whether they tried to add a review
  $addReviewError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "addReview" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["restaurantID"]) || $_POST["restaurantID"] == "" )
    {
      $addReviewError = "You must enter a restaurant ID.";
    }
    else if( !isset($_POST["addText"]) || $_POST["addText"] == "" )
    {
      $addReviewError = "Your review must have some text.";
    }
    else
    {
      // build the JSON objects
      $loginInfo = array("username" => $_SESSION["username"], "pwHash" => $_SESSION["pwHash"]);
      $JSON = array("loginInfo" => $loginInfo, "restaurantID" => $_POST["restaurantID"], "rating" => $_POST["addRating"],
                    "price" => $_POST["addPrice"], "text" => $_POST["addText"]);

      // call the API function
      $postReviewOutput = json_decode(PostReview(json_encode($JSON)), true);
      if( $postReviewOutput["feedback"] == "success" )
      {
        $addReviewError = "New review posted!";
      }
      else
      {
        $addReviewError = $postReviewOutput["feedback"];
      }
    }
  }

  // see whether they searched for anything
  $searchError = "&nbsp;";
  // restaurants by city and state
  if( isset($_POST["task"]) && $_POST["task"] == "citySearch" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["findCity"]) || $_POST["findCity"] == "" )
    {
      $searchError = "You must enter a city name.";
    }
    else if( !isset($_POST["findState"]) || $_POST["findState"] == "" )
    {
      $searchError = "You must enter a state abbreviation.";
    }
    else
    {
      // build the JSON
      $JSON = array("city" => $_POST["findCity"], "state" => $_POST["findState"]);

      // call the API function
      $searchResults = json_decode( GetRestaurantsByCity( json_encode($JSON) ), true);
      if( $searchResults["feedback"] != "success" )
      {
        $searchError = $searchResults["feedback"];
      }
    }
  }
  // restaurants by name
  else if( isset($_POST["task"]) && $_POST["task"] == "restaurantSearch" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["findRestaurant"]) || $_POST["findRestaurant"] == "" )
    {
      $searchError = "You must enter a restaurant name.";
    }
    else
    {
      // build the JSON
      $JSON = array("name" => $_POST["findRestaurant"]);

      // call the API function
      $searchResults = json_decode( GetRestaurantsByName( json_encode($JSON) ), true);
      if( $searchResults["feedback"] != "success" )
      {
        $searchError = $searchResults["feedback"];
      }
    }
  }
  // restaurants by distance
  else if( isset($_POST["task"]) && $_POST["task"] == "distanceSearch" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["findLatitude"]) || $_POST["findLatitude"] == "" )
    {
      $searchError = "You must enter a latitude.";
    }
    else if( !isset($_POST["findLatitudeDirection"]) || $_POST["findLatitudeDirection"] == "" )
    {
      $searchError = "You must enter a latitude direction.";
    }
    else if( !isset($_POST["findLongitude"]) || $_POST["findLongitude"] == "" )
    {
      $searchError = "You must enter a longitude.";
    }
    else if( !isset($_POST["findLongitudeDirection"]) || $_POST["findLongitudeDirection"] == "" )
    {
      $searchError = "You must enter a longitude direction.";
    }
    else
    {
      // build the JSON
      $JSON = array("latitude" => ($_POST["findLatitude"] * $_POST["findLatitudeDirection"]),
                    "longitude" => ($_POST["findLongitude"] * $_POST["findLongitudeDirection"]), "distanceInMiles" => 10);

      // call the API function
      $searchResults = json_decode( GetRestaurantsByDistance( json_encode($JSON) ), true);
      if( $searchResults["feedback"] != "success" )
      {
        $searchError = $searchResults["feedback"];
      }
    }
  }
  // reviews by username
  else if( isset($_POST["task"]) && $_POST["task"] == "userSearch" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["findUser"]) || $_POST["findUser"] == "" )
    {
      $searchError = "You must enter a username.";
    }
    else
    {
      // build the JSON
      $JSON = array("username" => $_POST["findUser"]);

      // call the API function
      $searchResults = json_decode( GetUsersByName( json_encode($JSON) ), true);
      if( $searchResults["feedback"] != "success" )
      {
        $searchError = $searchResults["feedback"];
      }
    }
  }

  // see if they tried to edit a review
  $editReviewError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "editReview" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["editReviewID"]) || $_POST["editReviewID"] == "" )
    {
      $editReviewError = "You must enter a review ID.";
    }
    else if( !isset($_POST["editText"]) || $_POST["editText"] == "" )
    {
      $editReviewError = "Your review must have some text.";
    }
    else
    {
      // build the JSON objects
      $loginInfo = array("username" => $_SESSION["username"], "pwHash" => $_SESSION["pwHash"]);
      $JSON = array("loginInfo" => $loginInfo, "reviewID" => $_POST["editReviewID"], "rating" => $_POST["editRating"],
                    "price" => $_POST["editPrice"], "text" => $_POST["editText"]);

      // call the API function
      $editReviewOutput = json_decode(EditReview(json_encode($JSON)), true);
      if( $editReviewOutput["feedback"] == "success" )
      {
        $editReviewError = "&nbsp;";
      }
      else
      {
        $editReviewError = $editReviewOutput["feedback"];
      }
    }
  }

  // see if they tried to delete a review
  $deleteReviewError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "deleteReview" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["deleteReviewID"]) || $_POST["deleteReviewID"] == "" )
    {
      $deleteReviewError = "You must enter a review ID.";
    }
    else
    {
      // build the JSON objects
      $loginInfo = array("username" => $_SESSION["username"], "pwHash" => $_SESSION["pwHash"]);
      $JSON = array("loginInfo" => $loginInfo, "reviewID" => $_POST["deleteReviewID"]);

      // call the API function
      $deleteReviewOutput = json_decode(DeleteReview(json_encode($JSON)), true);
      if( $deleteReviewOutput["feedback"] == "success" )
      {
        $deleteReviewError = "&nbsp;";
      }
      else
      {
        $deleteReviewError = $deleteReviewOutput["feedback"];
      }
    }
  }

  // see whether they requested a particular restaurant
  if( isset($_GET["showID"]) )
  {
    // build the JSON
    $showID = $_GET["showID"];
    $JSON = array("restaurantID" => $showID);

    $showRestaurantData = json_decode( GetRestaurantByID( json_encode($JSON) ), true);
    $showRestaurantData = $showRestaurantData["restaurant"];
  }

  // see whether they requested a particular users reviews
  if( isset($_GET["showReviews"]) )
  {
    // build the JSON
    $showReviewerID = $_GET["showReviews"];
    $JSON = array("userID" => $showReviewerID);

    $showReviewerData = json_decode( GetReviewsByUserID( json_encode($JSON) ), true);
  }
?>