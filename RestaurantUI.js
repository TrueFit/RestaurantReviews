  // This function lets us limit input fields to a certain character set
  function FilterField(id)
  {
    var allowed = "";
    if( id == "username" || id == "loginUsername" || id == "findUser" )
    {
      allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
    }
    else if( id == "email" )
    {
      allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.@-";
    }
    else if( id == "firstName" || id == "lastName" || id == "findCity" || id == "addCity" )
    {
      allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-.' ";
    }
    else if( id == "findState" || id == "addState" )
    {
      allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
    else if( id == "addLongitude" || id == "addLatitude" || id == "findLongitude" || id == "findLatitude" )
    {
      allowed = "0123456789.";
    }
    else if( id == "addPhone" || id == "addZip" )
    {
      allowed = "0123456789-";
    }
    else if( id == "addRating" || id == "addPrice" || id.substring(0, 10) == "editRating" || id.substring(0, 9) == "editPrice" )
    {
      allowed = "12345";
    }
    else
    {
      return;
    }

    var element = document.getElementById(id);
    var string = element.value;
    if( id == "findState" || id == "addState" )
    {
      string = string.toUpperCase();
    }
    for(var i=0; i<string.length; ++i)
    {
      if( allowed.indexOf(string.charAt(i)) == -1 )
      {
        string = string.substr(0,i).concat(string.substr(i+1));
        i--;
      }
    }
    element.value = string;
  }

  // This function toggles an element's display value between "inline" and "none"
  function Toggle(changeID)
  {
    var element = document.getElementById(changeID);
    if( element.style.display == "none" )
    {
      element.style.display = "inline";
    }
    else
    {
      element.style.display = "none";
    }
  }

  // this is a helper function that throws a dialog box up to confirm a user wants to delete a review or photo
  function ConfirmDelete(deletionFormID)
  {
    if( confirm("Are you sure you want to delete this?  Click cancel if not.") )
    {
      document.getElementById(deletionFormID).submit();
    }
  }

  // this is a helper function that populates a text box with the selected option in a different dropdown
  function CopyDropdownTo(dropdownID, targetID)
  {
    var e = document.getElementById(dropdownID);
    document.getElementById(targetID).value = e.options[e.selectedIndex].text;
  }

  // this is a helper function to rebuild a form on the admin page
  function LoadDivFromDropdown(type)
  {
    // see which form we are trying to load up
    var targetID = "";
    var srcURL = "";
    if( type == 'restaurant' )
    {
      var e = document.getElementById('editRestaurantID');
      targetID = 'restaurantData';
      srcURL = "LoadRestaurantDataForm.php?ID=" + e.options[e.selectedIndex].value;
    }

    // build the request object
    var xmlhttp;
    if (window.XMLHttpRequest)
    {//code for IE7+, Firefox, Chrome, Opera, Safari
      xmlhttp=new XMLHttpRequest();
    }
    else
    {
      xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.onreadystatechange=function()
    {
      if (xmlhttp.readyState==4 && xmlhttp.status==200)
      {
        // tack on the new elements
        document.getElementById(targetID).innerHTML = xmlhttp.responseText;
      }
    }

    // make the actual request
    xmlhttp.open("GET", srcURL, true);
    xmlhttp.send();
  }