<?php
  session_start();

  // check to see whether they tried to add a new restaurant type
  $addError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "addRestaurantType" )
  {
    // they gave us no type or an empty type
    if( !isset($_POST["addName"]) || $_POST["addName"] == "" )
    {
      $addError = "Must specify a restaurant type.";
    }
    // they sent us a type name, so pass it into the API.
    else
    {
      // build the JSON objects
      $JSON = array("name" => $_POST["addName"]);

      // call the API function
      $addTypeOutput = json_decode(AdminAddRestaurantType(json_encode($JSON)), true);
      if( $addTypeOutput["feedback"] == "success" )
      {
        $addError = "New type added!";
      }
      else
      {
        $addError = $addTypeOutput["feedback"];
      }
    }
  }

  // check to see whether they tried to edit a restaurant type
  $editTypeError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "editRestaurantType" )
  {
    // they gave us no type or an empty type
    if( !isset($_POST["editTypeName"]) || $_POST["editTypeName"] == "" )
    {
      $editTypeError = "Must specify a restaurant type name.";
    }
    // they gave us no ID or an empty ID
    else if( !isset($_POST["editTypeID"]) || $_POST["editTypeID"] == "" )
    {
      $editTypeError = "Must specify a restaurant type ID.";
    }
    // they sent us an edited type, so pass it into the API.
    else
    {
      // build the JSON objects
      $JSON = array("typeID" => $_POST["editTypeID"], "name" => $_POST["editTypeName"]);

      // call the API function
      $editTypeOutput = json_decode(AdminEditRestaurantType(json_encode($JSON)), true);
      if( $editTypeOutput["feedback"] == "success" )
      {
        $editTypeError = "Type changed!";
      }
      else
      {
        $editTypeError = $editTypeOutput["feedback"];
      }
    }
  }

  // check to see whether they tried to edit a restaurant
  $editRestaurantError = "&nbsp;";
  if( isset($_POST["task"]) && $_POST["task"] == "editRestaurant" )
  {
    // make sure they've provided all the required fields
    if( !isset($_POST["editRestaurantName"]) || $_POST["editRestaurantName"] == "" )
    {
      $editRestaurantError = "You must enter a restaurant name.";
    }
    else if( !isset($_POST["editRestaurantTypeID"]) || $_POST["editRestaurantTypeID"] == "" )
    {
      $editRestaurantError = "You must enter a restaurant type.";
    }
    else if( !isset($_POST["editRestaurantAddr1"]) || $_POST["editRestaurantAddr1"] == "" )
    {
      $editRestaurantError = "You must enter an address.";
    }
    else if( !isset($_POST["editRestaurantCity"]) || $_POST["editRestaurantCity"] == "" )
    {
      $editRestaurantError = "You must enter a city.";
    }
    else if( !isset($_POST["editRestaurantState"]) || $_POST["editRestaurantState"] == "" )
    {
      $editRestaurantError = "You must enter a state.";
    }
    else if( !isset($_POST["editRestaurantZip"]) || $_POST["editRestaurantZip"] == "" )
    {
      $editRestaurantError = "You must enter a zip code.";
    }
    // they must be ok.  send it to the API
    else
    {
      // build the JSON objects
      $location = array("addr1" => $_POST["editRestaurantAddr1"], "addr2" => $_POST["editRestaurantAddr2"], 
                        "city" => $_POST["editRestaurantCity"], "state" => $_POST["editRestaurantState"], 
                        "zip" => $_POST["editRestaurantZip"], "latitude" => $_POST["editRestaurantLatitude"], 
                        "longitude" => $_POST["editRestaurantLongitude"]);
      $JSON = array("restaurantID" => $_POST["editRestaurantID"], "name" => $_POST["editRestaurantName"], 
                    "isApproved" => $_POST["editRestaurantIsApproved"], "typeID" => $_POST["editRestaurantTypeID"], 
                    "location" => $location, "phoneNumber" => $_POST["editRestaurantPhone"], 
                    "websiteURL" => $_POST["editRestaurantWebsite"]);

      // call the API function
      $editRestaurantOutput = json_decode(AdminEditRestaurant(json_encode($JSON)), true);
      if( $editRestaurantOutput["feedback"] == "success" )
      {
        $editRestaurantError = "Restaurant updated!";
      }
      else
      {
        $editRestaurantError = $editRestaurantOutput["feedback"];
      }
    }
  }
?>