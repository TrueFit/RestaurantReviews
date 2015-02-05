<?php
  // include the restaurant API 
  //$isAdmin = true;
  $pathToDBCredentials = "../../../www_helm/truefit/restaurantDBAdminLogin.php";
  include_once "../RestaurantAPI.php";

  // include the preprocessing
  include "AdminProcessing.php";
?>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
  <head>
    <title>Truefit Admin Example</title>
    <script type="text/javascript" src="../RestaurantUI.js?cache=11"></script>
  </head>
  <body>
    <span style="font-weight:bold;">Restaurant Admin Page</span><br><br>
<?php

  // add the capability to add a new restaurant type
  $showAddType = (isset($_POST["task"]) && ($_POST["task"] == "addRestaurantType")) ? "inline" : "none";
  $addName = isset($_POST["addName"]) ? $_POST["addName"] : "";
?>
    <a href="#" onclick="Toggle('addTypeDiv'); return false;"><span style="font-weight:bold;">Add Restaurant Type</span></a><br>
    <div id="addTypeDiv" style="display:<?php echo $showAddType; ?>">
      <form action="AdminUI.php" method="post">
        <input type="hidden" name="task" value="addRestaurantType">
        <table>
          <tr>
            <td>Name</td>
            <td><input type="text" name="addName" id="addName" maxlength="80" value="<?php echo $addName; ?>"></td>
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

  // add the capability to edit a restaurant type
  $showEditType = (isset($_POST["task"]) && ($_POST["task"] == "editRestaurantType")) ? "inline" : "none";
  $editTypeName = isset($_POST["editTypeName"]) ? $_POST["editTypeName"] : "";
?>
    <a href="#" onclick="Toggle('editTypeDiv'); CopyDropdownTo('editTypeID', 'editTypeName'); return false;"><span style="font-weight:bold;">Edit Restaurant Type</span></a><br>
    <div id="editTypeDiv" style="display:<?php echo $showEditType; ?>">
      <form action="AdminUI.php" method="post">
        <input type="hidden" name="task" value="editRestaurantType">
        <table>
          <tr>
            <td>Types</td>
            <td><select name="editTypeID" id="editTypeID" onchange="CopyDropdownTo('editTypeID', 'editTypeName');">
<?php

  // grab all the available types
  $types = json_decode( GetRestaurantTypes(), true );
  for( $i=0; $i<count($types["types"]); $i++ )
  {
    $typeID = $types["types"][$i]["typeID"];
    $typeName = $types["types"][$i]["name"];
    $selected = (isset($_POST["editTypeID"]) && $_POST["editTypeID"] == $typeID) ? " selected" : "";
?>
              <option value="<?php echo ($typeID . "\"" . $selected . ">" . $typeName); ?></option>
<?php
  }
?>
            </select></td>
          </tr>
          <tr>
            <td>Name</td>
            <td><input type="text" name="editTypeName" id="editTypeName" maxlength="80" value="<?php echo $editTypeName; ?>"></td>
          </tr>
          <tr>
            <td colspan="2"><input type="submit" value="Edit"></td>
          </tr>
          <tr>
            <td colspan="2" style="color:#FF0000;"><?php echo $editTypeError; ?></td>
          </tr>
        </table>
      </form><br>
    </div><br>
<?php

  // add the capability to edit a restaurant
  $showEditRestaurant = (isset($_POST["task"]) && ($_POST["task"] == "editRestaurant")) ? "inline" : "none";
?>
    <a href="#" onclick="Toggle('editRestaurantDiv'); LoadDivFromDropdown('restaurant'); return false;"><span style="font-weight:bold;">Edit Restaurant</span></a><br>
    <div id="editRestaurantDiv" style="display:<?php echo $showEditRestaurant; ?>">
      <span>Restaurants</span>
      <select id="editRestaurantID" onchange="LoadDivFromDropdown('restaurant');">
<?php

  // grab all the available restaurants
  $name = array("name" => "%", "isAdmin" => "YES");
  $restaurants = json_decode( GetRestaurantsbyName( json_encode($name) ), true );
  for( $i=0; $i<count($restaurants["restaurants"]); $i++ )
  {
    $ID = $restaurants["restaurants"][$i]["ID"];
    $name = "ID #" . $ID . " " . $restaurants["restaurants"][$i]["name"];
    $selected = (isset($_POST["editRestaurantID"]) && $_POST["editRestaurantID"] == $ID) ? " selected" : "";
?>
        <option value="<?php echo ($ID . "\"" . $selected . ">" . $name); ?></option>
<?php
  }
?>
      </select><br>
      <div id="restaurantData">
<?php 

  // fill in the current data
  if( $showEditRestaurant )
  {
    include "LoadRestaurantDataForm.php";
  }
?>
      </div>
    </div><br>
  </body>
</html>
