<?php
  // include the restaurant API 
  $pathToDBCredentials = "../../www_helm/truefit/restaurantDBUserLogin.php";
  include "RestaurantAPI.php";

  // include the preprocessing
  include "RestaurantProcessing.php";
?>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
  <head>
    <title>Truefit Restaurant Example</title>
    <script type="text/javascript" src="RestaurantUI.js"></script>
  </head>
  <body>
<?php
  // see whether they are logged in or not
  if( isset($_SESSION["username"]) && isset($_SESSION["pwHash"]) )
  {
?>
    <span>Logged in as: <?php echo $_SESSION["username"]; ?></span><br>
    <form action="RestaurantUI.php" method="post">
      <input name="logout" value="true" type="hidden">
      <input value="Logout" type="submit">
    </form><br><br>
<?php
  }
  // they're not logged in, so show the UI for them to do so
  else
  {
    // make aliases for these so we're a bit more readable below
    $loginUN = isset($_POST["loginUsername"]) ? $_POST["loginUsername"] : "";
    $loginPW = isset($_POST["loginPassword"]) ? $_POST["loginPassword"] : "";
    $createUN = isset($_POST["username"]) ? $_POST["username"] : "";
    $createPW = isset($_POST["password"]) ? $_POST["password"] : "";
    $createConfirmPW = isset($_POST["confirmPW"]) ? $_POST["confirmPW"] : "";
    $createEmail = isset($_POST["email"]) ? $_POST["email"] : "";
    $createFirstName = isset($_POST["firstName"]) ? $_POST["firstName"] : "";
    $createLastName = isset($_POST["lastName"]) ? $_POST["lastName"] : "";
?>
    <table>
      <tr>
        <td style="vertical-align:top;"><form action="RestaurantUI.php" method="post">
          <input type="hidden" name="task" value="login">
          <table>
            <tr>
              <td colspan="2" style="text-align:center;">Login</td>
            </tr>
            <tr>
              <td>Username</td>
              <td><input name="loginUsername" id="loginUsername" type="text" value="<?php echo $loginUN; ?>" onkeyup="FilterField('loginUsername');"></td>
            </tr>
            <tr>
              <td>Password</td>
              <td><input name="loginPassword" type="password" value="<?php echo $loginPW; ?>"></td>
            </tr>
            <tr>
              <td colspan="2" style="text-align:center;"><input type="submit" value="Sign In"></td>
            </tr>
            <tr>
              <td colspan="2" style="text-align:center; color:#FF0000;"><?php echo $loginError; ?></td>
            </tr>
          </table>
        </form></td>
        <td style="width:50px;">&nbsp;</td>
        <td style="vertical-align:top;"><form action="RestaurantUI.php" method="post">
          <input type="hidden" name="task" value="createUser">
          <table>
            <tr>
              <td colspan="2" style="text-align:center;">Create New Account</td>
            </tr>
            <tr>
              <td>*Username</td>
              <td><input name="username" id="username" type="text" value="<?php echo $createUN; ?>" onkeyup="FilterField('username');"></td>
            </tr>
            <tr>
              <td>*Password</td>
              <td><input name="password" type="password" value="<?php echo $createPW; ?>"></td>
            </tr>
            <tr>
              <td>*Confirm Password</td>
              <td><input name="confirmPW" type="password" value="<?php echo $createConfirmPW; ?>"></td>
            </tr>
            <tr>
              <td>*Email</td>
              <td><input name="email" id="email" type="text" value="<?php echo $createEmail; ?>" onkeyup="FilterField('email');"></td>
            </tr>
            <tr>
              <td>First Name</td>
              <td><input name="firstName" id="firstName" type="text" value="<?php echo $createFirstName; ?>" onkeyup="FilterField('firstName');"></td>
            </tr>
            <tr>
              <td>Last Name</td>
              <td><input name="lastName" id="lastName" type="text" value="<?php echo $createLastName; ?>" onkeyup="FilterField('lastName');"></td>
            </tr>
            <tr>
              <td colspan="2" style="text-align:center;"><input type="submit" value="Sign Up"></td>
            </tr>
            <tr>
              <td colspan="2" style="text-align:center; color:#FF0000;"><?php echo $createError; ?></td>
            </tr>
          </table>
        </form></td>
      </tr>
    </table><br><br>
<?php
  }

  // add the capability to add a new restaurant
  $addDivStyle = (isset($_POST["task"]) && ($_POST["task"] == "addRestaurant")) ? "inline" : "none";
  $addNameValue = isset($_POST["addName"]) ? $_POST["addName"] : "";
?>
    <a href="#" onclick="Toggle('addDiv'); return false;"><span style="font-weight:bold;">Add Restaurant</span></a><br>
    <div id="addDiv" style="display:<?php echo $addDivStyle; ?>;">
      <form action="RestaurantUI.php" method="post">
        <input type="hidden" name="task" value="addRestaurant">
        <table>
          <tr>
            <td>Name</td>
            <td><input type="text" name="addName" id="addName" maxlength="80" value="<?php echo $addNameValue; ?>"></td>
          </tr>
          <tr>
            <td>Cuisine</td>
            <td><select name="addType">
<?php
  $types = json_decode( GetRestaurantTypes(), true );
  for( $i=0; $i<count($types["types"]); $i++ )
  {
    $typeID = $types["types"][$i]["typeID"];
    $typeName = $types["types"][$i]["name"];
    $selected = (isset($_POST["addType"]) && $_POST["addType"] == $typeID) ? " selected" : "";
?>
              <option value="<?php echo ($typeID . "\"" . $selected . ">" . $typeName); ?></option>
<?php
  }

  // more aliases for below
  $addAddr1Value = isset($_POST["addAddr1"]) ? $_POST["addAddr1"] : "";
  $addAddr2Value = isset($_POST["addAddr2"]) ? $_POST["addAddr2"] : "";
  $addCityValue = isset($_POST["addCity"]) ? $_POST["addCity"] : "";
  $addStateValue = isset($_POST["addState"]) ? $_POST["addState"] : "";
  $addZipValue = isset($_POST["addZip"]) ? $_POST["addZip"] : "";
  $addLatitudeValue = isset($_POST["addLatitude"]) ? $_POST["addLatitude"] : "";
  $addLatitudeDirectionValue = isset($_POST["addLatitudeDirection"]) ? $_POST["addLatitudeDirection"] : "";
  $addLongitudeValue = isset($_POST["addLongitude"]) ? $_POST["addLongitude"] : "";
  $addLongitudeDirectionValue = isset($_POST["addLongitudeDirection"]) ? $_POST["addLongitudeDirection"] : "";
  $addPhoneValue = isset($_POST["addPhone"]) ? $_POST["addPhone"] : "";
  $addWebsiteValue = isset($_POST["addWebsite"]) ? $_POST["addWebsite"] : "";
?>
            </select></td>
          </tr>
          <tr>
            <td>Address 1</td>
            <td><input type="text" name="addAddr1" id="addAddr1" maxlength="255" value="<?php echo $addAddr1Value; ?>"></td>
          </tr>
          <tr>
            <td>Address 2</td>
            <td><input type="text" name="addAddr2" id="addAddr2" maxlength="255" value="<?php echo $addAddr2Value; ?>"></td>
          </tr>
          <tr>
            <td>City</td>
            <td><input type="text" name="addCity" id="addCity" maxlength="40" value="<?php echo $addCityValue; ?>" onkeyup="FilterField('addCity');"></td>
          </tr>
          <tr>
            <td>State</td>
            <td><input type="text" name="addState" id="addState" maxlength="2" value="<?php echo $addStateValue; ?>" onkeyup="FilterField('addState');"></td>
          </tr>
          <tr>
            <td>Zip</td>
            <td><input type="text" name="addZip" id="addZip" maxlength="20" value="<?php echo $addZipValue; ?>" onkeyup="FilterField('addZip');"></td>
          </tr>
          <tr>
            <td>Latitude</td>
            <td>
              <input type="text" name="addLatitude" id="addLatitude" value="<?php echo $addLatitudeValue; ?>" onkeyup="FilterField('addLatitude');">
              <select name="addLatitudeDirection">
                <option value="1"<?php echo ($addLatitudeDirectionValue == 1) ? " selected" : ""; ?>>North</option>
                <option value="-1"<?php echo ($addLatitudeDirectionValue == -1) ? " selected" : ""; ?>>South</option>
              </select>
            </td>
          </tr>
          <tr>
            <td>Longitude</td>
            <td>
              <input type="text" name="addLongitude" id="addLongitude" value="<?php echo $addLongitudeValue; ?>" onkeyup="FilterField('addLongitude');">
              <select name="addLongitudeDirection">
                <option value="1"<?php echo ($addLongitudeDirectionValue == 1) ? " selected" : ""; ?>>East</option>
                <option value="-1"<?php echo ($addLongitudeDirectionValue == -1) ? " selected" : ""; ?>>West</option>
              </select>
            </td>
          </tr>
          <tr>
            <td>Phone</td>
            <td><input type="text" name="addPhone" id="addPhone" value="<?php echo $addPhoneValue; ?>" onkeyup="FilterField('addPhone');"></td>
          </tr>
          <tr>
            <td>Website</td>
            <td><input type="text" name="addWebsite" id="addWebsite" value="<?php echo $addWebsiteValue; ?>"></td>
          </tr>
          <tr>
            <td colspan="2"><input type="submit" value="Add"></td>
          </tr>
          <tr>
            <td colspan="2" style="color:#FF0000;"><?php echo $addError; ?></td>
          </tr>
        </table>
      </form><br>
    </div><br>
<?php

  // add the capability to search for either users of restaurants
  $searchDivDisplay = (isset($_POST["task"]) && ($_POST["task"] == "userSearch" || $_POST["task"] == "restaurantSearch" || 
                                                 $_POST["task"] == "citySearch" || $_POST["task"] == "distanceSearch")) ? "inline" : "none";
  $findUserValue = isset($_POST["findUser"]) ? $_POST["findUser"] : "";
  $findRestaurantValue = isset($_POST["findRestaurant"]) ? $_POST["findRestaurant"] : "";
  $findCityValue = isset($_POST["findCity"]) ? $_POST["findCity"] : "";
  $findStateValue = isset($_POST["findState"]) ? $_POST["findState"] : "";
  $findLatitudeValue = isset($_POST["findLatitude"]) ? $_POST["findLatitude"] : "";
  $findLatitudeDirectionValue = isset($_POST["findLatitudeDirection"]) ? $_POST["findLatitudeDirection"] : "";
  $findLongitudeValue = isset($_POST["findLongitude"]) ? $_POST["findLongitude"] : "";
  $findLongitudeDirectionValue = isset($_POST["findLongitudeDirection"]) ? $_POST["findLongitudeDirection"] : "";
?>
    <a href="#" onclick="Toggle('searchDiv'); return false;"><span style="font-weight:bold;">Search</span></a><br>
    <div id="searchDiv" style="display:<?php echo $searchDivDisplay; ?>;">
      <form action="RestaurantUI.php" method="post">
        <input type="hidden" name="task" value="userSearch">
        <span>reviews by username</span>
        <input type="text" name="findUser" id="findUser" value="<?php echo $findUserValue; ?>" onkeyup="FilterField('findUser');">
        <input type="submit" value="Search">
      </form><br>
      <form action="RestaurantUI.php" method="post">
        <input type="hidden" name="task" value="restaurantSearch">
        <span>by restaurant name</span>
        <input type="text" name="findRestaurant" id="findRestaurant" value="<?php echo $findRestaurantValue; ?>">
        <input type="submit" value="Search">
      </form><br>
      <form action="RestaurantUI.php" method="post">
        <input type="hidden" name="task" value="citySearch">
        <span>by city</span>
        <input type="text" name="findCity" id="findCity" value="<?php echo $findCityValue; ?>" onkeyup="FilterField('findCity');">
        <span>in state</span>
        <input type="text" name="findState" id="findState" value="<?php echo $findStateValue; ?>" onkeyup="FilterField('findState');" maxlength="2" style="width:30px;">
        <input type="submit" value="Search">
      </form><br>
      <form action="RestaurantUI.php" method="post">
        <input type="hidden" name="task" value="distanceSearch">
        <span>by latitude</span>
        <input type="text" name="findLatitude" id="findLatitude" value="<?php echo $findLatitudeValue; ?>" onkeyup="FilterField('findLatitude');">
        <select name="findLatitudeDirection">
          <option value="1"<?php echo ($findLatitudeDirectionValue == 1) ? " selected" : ""; ?>>North</option>
          <option value="-1"<?php echo ($findLatitudeDirectionValue == -1) ? " selected" : ""; ?>>South</option>
        </select>
        <span>and longitude</span>
        <input type="text" name="findLongitude" id="findLongitude" value="<?php echo $findLongitudeValue; ?>" onkeyup="FilterField('findLongitude');">
        <select name="findLongitudeDirection">
          <option value="1"<?php echo ($findLongitudeDirectionValue == 1) ? " selected" : ""; ?>>East</option>
          <option value="-1"<?php echo ($findLongitudeDirectionValue == -1) ? " selected" : ""; ?>>West</option>
        </select>
        <input type="submit" value="Search">
      </form><br>
      <span style="color:#FF0000;"><?php echo $searchError; ?></span><br><br>
<?php

  // they searched and there was an error.  show no results
  if( $searchError != "&nbsp;" )
  {
  }
  // they searched for reviews by user, so show the results
  else if( isset($_POST["task"]) && $_POST["task"] == "userSearch" )
  {
    echo "      <br><span style=\"font-weight:bold;\">Username Search Results for:</span><span>" . $_POST["findUser"] . "</span><br><br>\n";
    for( $i=0; $i<count($searchResults["users"]); $i++ )
    {
      $row = $searchResults["users"][$i];
      echo "      <span>User <a href=\"RestaurantUI.php?showReviews=" . $row["userID"] . "\">" . $row["username"] . "</a> has written " . 
          $row["reviewCount"] . " review" . (($row["reviewCount"] == 1) ? "" : "s") . "</span><br>\n";
    }
  }
  // they searched for restaurants, so show the results
  else if( isset($_POST["task"]) && ($_POST["task"] == "restaurantSearch" || $_POST["task"] == "citySearch" || 
                                     $_POST["task"] == "distanceSearch") )
  {
    // determine what header we need to put on these results
    $headerText = "Restaurant Search Results for: ";
    if( $_POST["task"] == "restaurantSearch" )
    {
      $headerText .= $findRestaurantValue;
    }
    else if( $_POST["task"] == "citySearch" )
    {
      $headerText .= $findCityValue . ", " . $findStateValue;
    }
    else if( $_POST["task"] == "distanceSearch" )
    {
      $headerText .= "Within 10 miles of " . $findLatitudeValue . (($findLatitudeDirectionValue == 1) ? "N" : "S") . ", " . 
                     $findLongitudeValue . (($findLongitudeDirectionValue == 1) ? "E" : "W");
    }

    // show the header
?>
      <br><span style="font-weight:bold;"><?php echo $headerText; ?></span><br><br>
<?php
    // now cycle through the results
    for($i=0; $i<count($searchResults["restaurants"]); $i++)
    {
      $place = $searchResults["restaurants"][$i];
      $total = $place["rating"]["total"];
      $count = $place["rating"]["count"];
      $ratingText = (($count == 0) ? "No ratings yet" : (($total / $count) . " out of 5 (" . $count . " ratings)"));
      $total = $place["price"]["total"];
      $count = $place["price"]["count"];
      $priceText = (($count == 0) ? "No ratings yet" : (($total / $count) . " out of 5 (" . $count . " ratings)"));

      // if this is the distance search, show how far away it is
      if( $_POST["task"] == "distanceSearch" )
      {
        echo "      <span>" . $place["location"]["distanceInMiles"] . " miles away:</span><br>\n";
      }

      // dump out the address
      echo "      <a href=\"RestaurantUI.php?showID=" . $place["ID"] . "\">" . $place["name"] . "</a><br>\n";
      echo "      <span>" . $place["location"]["addr1"] . "</span><br>\n";
      if( $place["location"]["addr2"] != "null" && $place["location"]["addr2"] != "" )
      {
        echo "      <span>" . $place["location"]["addr2"] . "</span><br>\n";
      }
      echo "      <span>" . $place["location"]["city"] . ", " . $place["location"]["state"] . " " . $place["location"]["zip"] . "</span><br>\n";
      echo "      <span>Style: " . $place["typeName"] . "</span><br>\n";
      echo "      <span>Quality: " . $ratingText . "</span><br>\n";
      echo "      <span>Price: " . $priceText . "</span><br>\n";
      echo "      <span>" . count($place["reviews"]) . " review" . ((count($place["reviews"]) == 1) ? "" : "s") ."<br><br>\n";
    }
  }

  // end the search div
?>
    </div><br>
<?php

  // if they specified a restaurant they want to see, show it
  if( isset($showID) )
  {
    $total = $showRestaurantData["rating"]["total"];
    $count = $showRestaurantData["rating"]["count"];
    $ratingText = (($count == 0) ? "No ratings yet" : (($total / $count) . " out of 5 (" . $count . " ratings)"));
    $total = $showRestaurantData["price"]["total"];
    $count = $showRestaurantData["price"]["count"];
    $priceText = (($count == 0) ? "No ratings yet" : (($total / $count) . " out of 5 (" . $count . " ratings)"));

    echo "    <span style=\"font-weight:bold; font-size:200%;\">" . $showRestaurantData["name"] . "</span><br>";
    echo "    <span>" . $showRestaurantData["location"]["addr1"] . "</span><br>\n";
    if( $showRestaurantData["location"]["addr2"] != "null" && $showRestaurantData["location"]["addr2"] != "" )
    {
      echo "    <span>" . $showRestaurantData["location"]["addr2"] . "</span><br>\n";
    }
    echo "    <span>" . $showRestaurantData["location"]["city"] . ", " . $showRestaurantData["location"]["state"] . " " . 
        $showRestaurantData["location"]["zip"] . "</span><br>\n";
    echo "    <span>Style: " . $showRestaurantData["typeName"] . "<br>\n";
    echo "    <span>Quality: " . $ratingText . "<br>\n";
    echo "    <span>Price: " . $priceText . "<br>\n";
    echo "    <span>" . count($showRestaurantData["reviews"]) . " review" . ((count($showRestaurantData["reviews"]) == 1) ? "" : "s");
    if( isset($_SESSION["username"]) && isset($_SESSION["pwHash"]) )
    {
      $showAddDiv = (isset($addReviewError) && ($addReviewError != "&nbsp;")) ? "inline" : "none";
      $addRating = isset($_POST["addRating"]) ? $_POST["addRating"] : "";
      $addPrice = isset($_POST["addPrice"]) ? $_POST["addPrice"] : "";
      $addText = isset($_POST["addText"]) ? $_POST["addText"] : "";
?>
&nbsp;&nbsp;<a href="#" onclick="Toggle('addNewReview'); return false;"><span style="font-weight:bold;">Add Review</span></a><br>
      <div id="addNewReview" style="display:<?php echo $showAddDiv; ?>"><br>
      <form action="RestaurantUI.php?showID=<?php echo $showID; ?>" method="post">
        <input type="hidden" name="task" value="addReview">
        <input type="hidden" name="restaurantID" value="<?php echo $showID; ?>">
        <table>
          <tr>
            <td>Rating (low 1-5 high)</td>
            <td><input type="text" name="addRating" id="addRating" value="<?php echo $addRating; ?>" onkeyup="FilterField('addRating');"></td>
          </tr>
          <tr>
            <td>Price (low 1-5 high)</td>
            <td><input type="text" name="addPrice" id="addPrice" value="<?php echo $addPrice; ?>" onkeyup="FilterField('addPrice');"></td>
          </tr>
          <tr>
            <td>Your Review</td>
            <td><textarea name="addText" id="addText" style="resize:none; height:80px; width:400px;"><?php echo $addText; ?></textarea></td>
          </tr>
          <tr>
            <td colspan="2"><input type="submit" value="Add"></td>
          </tr>
          <tr>
            <td colspan="2" style="color:#FF0000;"><?php echo $addReviewError; ?></td>
          </tr>
        </table>
      </form><br>
    </div>
<?php

    }
    echo "<br><br><br>\n";

    // show all the reviews
    for( $i=0; $i<count($showRestaurantData["reviews"]); $i++ )
    {
      $thisReview = $showRestaurantData["reviews"][$i];
      echo "      <span><a href=\"RestaurantUI.php?showReviews=" . $thisReview["userID"] . "\">" . $thisReview["username"] . 
          "</a> says at " . strftime("%I:%M %p on %b %e, %Y", strtotime($thisReview["postTime"]));
      // if they're logged in and this is their review, let them edit or delete it
      if( isset($_SESSION["username"]) && isset($_SESSION["pwHash"]) && $_SESSION["username"] == $thisReview["username"] )
      {
        echo "&nbsp;&nbsp;<a href=\"#\" onclick=\"Toggle('currentReview" . $thisReview["reviewID"] . "'); Toggle('editReview" . $thisReview["reviewID"] . 
            "'); return false;\">Edit</a>";
        echo "&nbsp;&nbsp;<a href=\"#\" onclick=\"ConfirmDelete('deleteReview" . $thisReview["reviewID"] . "'); return false;\">Delete</a>";
      }
      echo "</span><br>\n";
      echo "      <div id=\"currentReview" . $thisReview["reviewID"] . "\" style=\"display:" . 
          ((isset($editReviewError) && ($editReviewError != "&nbsp;") && 
            isset($_POST["editReviewID"]) && ($_POST["editReviewID"] == $thisReview["reviewID"])) 
           ? "none" : "inline") . "\">\n";
      echo "        <span>Rating: " . (isset($thisReview["rating"]) ? ($thisReview["rating"] . " of 5") : "not rated") . "</span><br>\n";
      echo "        <span>Price: " . (isset($thisReview["price"]) ? ($thisReview["price"] . " of 5") : "not rated") . "</span><br><br>\n";
      echo "        <span>" . $thisReview["text"] . "</span><br><br><br>\n";
      echo "      </div>\n";

      // edit div and delete form
      if( isset($_SESSION["username"]) && isset($_SESSION["pwHash"]) && $_SESSION["username"] == $thisReview["username"] )
      {
        $showEditDiv = (isset($editReviewError) && ($editReviewError != "&nbsp;") && 
                        isset($_POST["editReviewID"]) && ($_POST["editReviewID"] == $thisReview["reviewID"])) 
                       ? "inline" : "none";
        $rating = (isset($_POST["editRating"]) && ($showEditDiv == "inline")) 
                  ? $_POST["editRating"] : (isset($thisReview["rating"]) ? $thisReview["rating"] : "");
        $price = (isset($_POST["editPrice"]) && ($showEditDiv == "inline")) 
                 ? $_POST["editPrice"] : (isset($thisReview["price"]) ? $thisReview["price"] : "");
        $text = (isset($_POST["editText"]) && ($showEditDiv == "inline")) 
                ? $_POST["editText"] : $thisReview["text"];
        $error = ($showEditDiv == "inline") ? $editReviewError : "&nbsp;";
?>
      <div id="editReview<?php echo $thisReview["reviewID"]; ?>" style="display:<?php echo $showEditDiv; ?>">
        <form action="RestaurantUI.php?showID=<?php echo $showID; ?>" method="post">
          <input type="hidden" name="task" value="editReview">
          <input type="hidden" name="editReviewID" value="<?php echo $thisReview["reviewID"]; ?>">
          <table>
            <tr>
              <td>Rating:</td>
              <td><input name="editRating" id="editRating<?php echo $thisReview["reviewID"]; ?>" type="text" value="<?php echo $rating; ?>" onkeyup="FilterField('editRating<?php echo $thisReview["reviewID"]; ?>');"> out of 5</td>
            </tr>
            <tr>
              <td>Price:</td>
              <td><input name="editPrice" id="editPrice<?php echo $thisReview["reviewID"]; ?>" type="text" value="<?php echo $price; ?>" onkeyup="FilterField('editPrice<?php echo $thisReview["reviewID"]; ?>');"> out of 5</td>
            </tr>
            <tr>
              <td>Your Review</td>
              <td><textarea name="editText" id="editText" style="resize:none; height:80px; width:400px;"><?php echo $text; ?></textarea></td>
            </tr>
            <tr>
              <td colspan="2"><input type="submit" value="Edit"></td>
            </tr>
            <tr>
              <td colspan="2" style="color:#FF0000;"><?php echo $error; ?></td>
            </tr>
          </table>
        </form><br>
      </div>
      <form action="RestaurantUI.php?showID=<?php echo $showID; ?>" method="post" id="deleteReview<?php echo $thisReview["reviewID"]; ?>">
        <input type="hidden" name="task" value="deleteReview">
        <input type="hidden" name="deleteReviewID" value="<?php echo $thisReview["reviewID"]; ?>">
      </form><br>
<?php
      }
    }
  }
  // if they specified a reviewer they want to see, show it
  else if( isset($showReviewerID) )
  {
    echo "    <span style=\"font-weight:bold; font-size:200%;\">" . count($showReviewerData["reviews"]) . " reviews by " . 
        $showReviewerData["username"] . "</span><br><br>";

    // iterate over the set of reviews
    for( $i=0; $i<count($showReviewerData["reviews"]); $i++ )
    {
      $thisReview = $showReviewerData["reviews"][$i];
      $thisRestaurant = $thisReview["restaurant"];
      echo "    <span style=\"font-weight:bold; font-size:150%;\">" . $thisRestaurant["name"] . "</span><br>";
      echo "    <span>" . $thisRestaurant["location"]["addr1"] . "</span><br>\n";
      if( $thisRestaurant["location"]["addr2"] != "null" && $thisReview["thisRestaurant"]["addr2"] != "" )
      {
        echo "    <span>" . $thisRestaurant["location"]["addr2"] . "</span><br>\n";
      }
      echo "    <span>" . $thisRestaurant["location"]["city"] . ", " . $thisRestaurant["location"]["state"] . " " . 
          $thisRestaurant["location"]["zip"] . "</span><br>\n";
      echo "    <span>Style: " . $thisRestaurant["typeName"] . "</span><br>\n";
      echo "    <span>posted at " . strftime("%I:%M %p on %b %e, %Y", strtotime($thisReview["postTime"]));

      // if they're logged in and this is their review, let them edit or delete it
      if( isset($_SESSION["username"]) && isset($_SESSION["pwHash"]) && $_SESSION["username"] == $showReviewerData["username"] )
      {
        echo "&nbsp;&nbsp;<a href=\"#\" onclick=\"Toggle('currentReview" . $thisReview["reviewID"] . "'); Toggle('editReview" . $thisReview["reviewID"] . 
            "'); return false;\">Edit</a>";
        echo "&nbsp;&nbsp;<a href=\"#\" onclick=\"ConfirmDelete('deleteReview" . $thisReview["reviewID"] . "'); return false;\">Delete</a>";
      }
      echo "</span><br>\n";
      echo "      <div id=\"currentReview" . $thisReview["reviewID"] . "\" style=\"display:" . 
          ((isset($editReviewError) && ($editReviewError != "&nbsp;") && 
            isset($_POST["editReviewID"]) && ($_POST["editReviewID"] == $thisReview["reviewID"])) 
           ? "none" : "inline") . "\">\n";
      echo "        <span>Rating: " . (isset($thisReview["rating"]) ? ($thisReview["rating"] . " of 5") : "not rated") . "</span><br>\n";
      echo "        <span>Price: " . (isset($thisReview["price"]) ? ($thisReview["price"] . " of 5") : "not rated") . "</span><br><br>\n";
      echo "        <span>" . $thisReview["text"] . "</span><br><br><br>\n";
      echo "      </div>\n";

      // edit div and delete form
      if( isset($_SESSION["username"]) && isset($_SESSION["pwHash"]) && $_SESSION["username"] == $showReviewerData["username"] )
      {
        $showEditDiv = (isset($editReviewError) && ($editReviewError != "&nbsp;") && 
                        isset($_POST["editReviewID"]) && ($_POST["editReviewID"] == $thisReview["reviewID"])) 
                       ? "inline" : "none";
        $rating = (isset($_POST["editRating"]) && ($showEditDiv == "inline")) 
                  ? $_POST["editRating"] : (isset($thisReview["rating"]) ? $thisReview["rating"] : "");
        $price = (isset($_POST["editPrice"]) && ($showEditDiv == "inline")) 
                 ? $_POST["editPrice"] : (isset($thisReview["price"]) ? $thisReview["price"] : "");
        $text = (isset($_POST["editText"]) && ($showEditDiv == "inline")) 
                ? $_POST["editText"] : $thisReview["text"];
        $error = ($showEditDiv == "inline") ? $editReviewError : "&nbsp;";
?>
      <div id="editReview<?php echo $thisReview["reviewID"]; ?>" style="display:<?php echo $showEditDiv; ?>">
        <form action="RestaurantUI.php?showReviews=<?php echo $showReviewerID; ?>" method="post">
          <input type="hidden" name="task" value="editReview">
          <input type="hidden" name="editReviewID" value="<?php echo $thisReview["reviewID"]; ?>">
          <table>
            <tr>
              <td>Rating:</td>
              <td><input name="editRating" id="editRating<?php echo $thisReview["reviewID"]; ?>" type="text" value="<?php echo $rating; ?>" onkeyup="FilterField('editRating<?php echo $thisReview["reviewID"]; ?>');"> out of 5</td>
            </tr>
            <tr>
              <td>Price:</td>
              <td><input name="editPrice" id="editPrice<?php echo $thisReview["reviewID"]; ?>" type="text" value="<?php echo $price; ?>" onkeyup="FilterField('editPrice<?php echo $thisReview["reviewID"]; ?>');"> out of 5</td>
            </tr>
            <tr>
              <td>Your Review</td>
              <td><textarea name="editText" id="editText" style="resize:none; height:80px; width:400px;"><?php echo $text; ?></textarea></td>
            </tr>
            <tr>
              <td colspan="2"><input type="submit" value="Edit"></td>
            </tr>
            <tr>
              <td colspan="2" style="color:#FF0000;"><?php echo $error; ?></td>
            </tr>
          </table>
        </form><br>
      </div>
      <form action="RestaurantUI.php?showReviews=<?php echo $showReviewerID; ?>" method="post" id="deleteReview<?php echo $thisReview["reviewID"]; ?>">
        <input type="hidden" name="task" value="deleteReview">
        <input type="hidden" name="deleteReviewID" value="<?php echo $thisReview["reviewID"]; ?>">
      </form><br>
<?php
      }
    }
  }
?>
  </body>
</html>
