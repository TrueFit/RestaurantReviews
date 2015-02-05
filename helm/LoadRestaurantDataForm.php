<?php
  // include the restaurant API 
  $isAdmin = true;
  $pathToDBCredentials = "../../../www_helm/truefit/restaurantDBAdminLogin.php";
  include_once "../RestaurantAPI.php";

  // initialize all the values
  $ID = isset($_POST["editRestaurantID"]) ? $_POST["editRestaurantID"] : "";
  $name = isset($_POST["editRestaurantName"]) ? $_POST["editRestaurantName"] : "";
  $typeID = isset($_POST["editRestaurantTypeID"]) ? $_POST["editRestaurantTypeID"] : "";
  $addr1 = isset($_POST["editRestaurantAddr1"]) ? $_POST["editRestaurantAddr1"] : "";
  $addr2 = isset($_POST["editRestaurantAddr2"]) ? $_POST["editRestaurantAddr2"] : "";
  $city = isset($_POST["editRestaurantCity"]) ? $_POST["editRestaurantCity"] : "";
  $state = isset($_POST["editRestaurantState"]) ? $_POST["editRestaurantState"] : "";
  $zip = isset($_POST["editRestaurantZip"]) ? $_POST["editRestaurantZip"] : "";
  $latitude = isset($_POST["editRestaurantLatitude"]) ? $_POST["editRestaurantLatitude"] : "";
  $longitude = isset($_POST["editRestaurantLongitude"]) ? $_POST["editRestaurantLongitude"] : "";
  $phone = isset($_POST["editRestaurantPhone"]) ? $_POST["editRestaurantPhone"] : "";
  $website = isset($_POST["editRestaurantWebsite"]) ? $_POST["editRestaurantWebsite"] : "";
  $isApproved = isset($_POST["editRestaurantIsApproved"]) ? $_POST["editRestaurantIsApproved"] : "";

  // see if they passed in an ID to load up
  if( isset($_GET["ID"]) )
  {
    $ID = $_GET["ID"];
    $JSON = array("restaurantID" => $ID, "isAdmin" => "YES" );
    $data = json_decode( GetRestaurantByID( json_encode($JSON) ), true );

    // replace these with the loaded parameters
    $name = $data["restaurant"]["name"];
    $typeID = $data["restaurant"]["typeID"];
    $addr1 = $data["restaurant"]["location"]["addr1"];
    $addr2 = $data["restaurant"]["location"]["addr2"];
    $city = $data["restaurant"]["location"]["city"];
    $state = $data["restaurant"]["location"]["state"];
    $zip = $data["restaurant"]["location"]["zip"];
    $latitude = $data["restaurant"]["location"]["latitude"];
    $longitude = $data["restaurant"]["location"]["longitude"];
    $phone = $data["restaurant"]["phoneNumber"];
    $website = $data["restaurant"]["websiteURL"];
    $isApproved = $data["restaurant"]["isApproved"];
  }

?>
      <form action="AdminUI.php" method="post">
        <input type="hidden" name="task" value="editRestaurant">
        <input type="hidden" name="editRestaurantID" value="<?php echo $ID; ?>">
        <table>
          <tr>
            <td>RestaurantID</td>
            <td><?php echo $ID; ?></td>
          </tr>
          <tr>
            <td>Name</td>
            <td><input name="editRestaurantName" id="editRestaurantName" value="<?php echo $name; ?>"</td>
          </tr>
          <tr>
            <td>Is Approved</td>
            <td><select name="editRestaurantIsApproved" id="editRestaurantIsApproved">
              <option value="Y"<?php echo (($isApproved == "Y") ? " selected" : ""); ?>>Yes</option>
              <option value="U"<?php echo (($isApproved == "U") ? " selected" : ""); ?>>Unknown</option>
              <option value="N"<?php echo (($isApproved == "N") ? " selected" : ""); ?>>No</option>
            </select></td>
          </tr>
          <tr>
            <td>Cuisine</td>
            <td><select name="editRestaurantTypeID">
<?php
  $types = json_decode( GetRestaurantTypes(), true );
  for( $i=0; $i<count($types["types"]); $i++ )
  {
    $loopTypeID = $types["types"][$i]["typeID"];
    $loopTypeName = $types["types"][$i]["name"];
    $selected = ($loopTypeID == $typeID) ? " selected" : "";
?>
              <option value="<?php echo ($loopTypeID . "\"" . $selected . ">" . $loopTypeName); ?></option>
<?php
  }
?>
            </select></td>
          </tr>
          <tr>
            <td>Address 1</td>
            <td><input type="text" name="editRestaurantAddr1" id="editRestaurantAddr1" maxlength="255" value="<?php echo $addr1; ?>"></td>
          </tr>
          <tr>
            <td>Address 2</td>
            <td><input type="text" name="editRestaurantAddr2" id="editRestaurantAddr2" maxlength="255" value="<?php echo $addr2; ?>"></td>
          </tr>
          <tr>
            <td>City</td>
            <td><input type="text" name="editRestaurantCity" id="editRestaurantCity" maxlength="40" value="<?php echo $city; ?>" onkeyup="FilterField('editRestaurantCity');"></td>
          </tr>
          <tr>
            <td>State</td>
            <td><input type="text" name="editRestaurantState" id="editRestaurantState" maxlength="2" value="<?php echo $state; ?>" onkeyup="FilterField('editRestaurantState');"></td>
          </tr>
          <tr>
            <td>Zip</td>
            <td><input type="text" name="editRestaurantZip" id="editRestaurantZip" maxlength="20" value="<?php echo $zip; ?>" onkeyup="FilterField('editRestaurantZip');"></td>
          </tr>
          <tr>
            <td>Latitude</td>
            <td>
              <input type="text" name="editRestaurantLatitude" id="editRestaurantLatitude" value="<?php echo $latitude; ?>" onkeyup="FilterField('editRestaurantLatitude');">
            </td>
          </tr>
          <tr>
            <td>Longitude</td>
            <td>
              <input type="text" name="editRestaurantLongitude" id="editRestaurantLongitude" value="<?php echo $longitude; ?>" onkeyup="FilterField('editRestaurantLongitude');">
            </td>
          </tr>
          <tr>
            <td>Phone</td>
            <td><input type="text" name="editRestaurantPhone" id="editRestaurantPhone" value="<?php echo $phone; ?>" onkeyup="FilterField('editRestaurantPhone');"></td>
          </tr>
          <tr>
            <td>Website</td>
            <td><input type="text" name="editRestaurantWebsite" id="editRestaurantWebsite" value="<?php echo $website; ?>"></td>
          </tr>
          <tr>
            <td colspan="2"><input type="submit" value="Edit"></td>
          </tr>
          <tr>
            <td colspan="2" style="color:#FF0000;"><?php echo $editRestaurantError; ?></td>
          </tr>
        </table>
      </form><br>
