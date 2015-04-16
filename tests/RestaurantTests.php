<?php
/**
 * Description of RestaurantTests
 *
 * @author john
 */

namespace truefitTests;

class RestaurantTests extends \PHPUnit_Framework_TestCase {
    
    public function runAll() {
        $this->addTest();
        $this->detailsTest();
    }
    
    public function addTest() {
        $payload = "{\"name\":\"Sharp Edge\",\"storeName\":\"Bistro on Penn\",\"cityId\":\"1\",\"city\":\"Pittsburgh\",
	\"address\":\"922 Penn Avenue\",\"zipCode\":\"15222\",\"phone\":\"412-338-2422\",\"facebookPage\":null,
	\"twitterHandle\":null,\"hours\":
	[
		{\"day\":\"2\",\"openHour\":\"11:00:00\",\"closeHour\":\"00:00:00\"},
		{\"day\":\"3\",\"openHour\":\"11:00:00\",\"closeHour\":\"00:00:00\"},
		{\"day\":\"4\",\"openHour\":\"11:00:00\",\"closeHour\":\"00:00:00\"},
		{\"day\":\"5\",\"openHour\":\"11:00:00\",\"closeHour\":\"00:00:00\"},
		{\"day\":\"6\",\"openHour\":\"11:00:00\",\"closeHour\":\"01:00:00\"},
		{\"day\":\"7\",\"openHour\":\"09:00:00\",\"closeHour\":\"01:00:00\"},
		{\"day\":\"1\",\"openHour\":\"09:00:00\",\"closeHour\":\"00:00:00\"}
	]}";
        $target = new \truefit\RestaurantHandler($payload);
        $target->add();
    }
    
    public function detailsTest() {
        $target = new \truefit\RestaurantHandler("");
        $target->details(1);
    }
}

?>
