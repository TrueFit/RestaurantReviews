<?php
/*
 * Takes care of CRUD operations for restaurants
 */

/**
 * Description of Restaurant
 * @package DAL
 * @author john
 */

namespace truefit\dal;

use \ORM;
use Exception;

class RestaurantDB extends DB {
    
    public function __construct()
    {
        parent::__construct();
    }
    
    /**
     * gets the restaurant that matched the passed $id
     * returns one Restaurant object or null if not found
     */
    public function byId($restaurantid)
    {
        /*
        select r.name,r.store_name,c.id as city_id, c.name as city_name,r.address,r.zip_code,r.phone,r.fb_page,r.twitter_handle,h.day,h.open_hour,h.close_hour
        from restaurant r
        join city c
        on c.id = r.city_id
        right join hours h
        on r.id = h.restaurant_id
        where r.id = $restaurantid
         */
        $result = ORM::for_table('restaurant')
                ->select_many('restaurant.name','restaurant.store_name','restaurant.address',
                        'restaurant.zip_code','restaurant.phone','restaurant.fb_page','restaurant.twitter_handle',
                        array('city_id' => 'city.id'), array('city_name' => 'city.name'), 
                        'hours.id','hours.day','hours.open_hour','hours.close_hour')
                ->join('city', array('restaurant.city_id', '=', 'city.id'))
                ->right_outer_join('hours', array('restaurant.id', '=', 'hours.restaurant_id'))
                ->where('restaurant.id', $restaurantid)
                ->find_many();
        if($result) {
            $foundRestaurant = new \truefit\model\Restaurant();
            $foundRestaurant->id = $restaurantid;
            $foundRestaurant->name = $result[0]->name;
            $foundRestaurant->storeName = $result[0]->store_name;
            $foundRestaurant->address = $result[0]->address;
            $foundRestaurant->zipCode = $result[0]->zip_code;
            $foundRestaurant->phone = $result[0]->phone;
            $foundRestaurant->facebookPage = $result[0]->fb_page;
            $foundRestaurant->twitterHandle = $result[0]->twitter_handle;
            $foundRestaurant->cityId = $result[0]->city_id;
            $foundRestaurant->city = $result[0]->city_name;
            $foundRestaurant->hours = array();
            foreach($result as $row) {
                $bussinessHours = new \truefit\model\BusinessHour();
                $bussinessHours->id = $row->id;
                $bussinessHours->day = $row->day;
                $bussinessHours->openHour = $row->open_hour;
                $bussinessHours->closeHour = $row->close_hour;
                $foundRestaurant->hours[] = $bussinessHours;
            }
            return $foundRestaurant;
        } else
            return null;
    }
    
    /**
     * 
     */
    public function byCity($cityId) {
        /*
        select r.name,r.store_name,c.name as city_name,r.address,r.zip_code,r.phone,r.fb_page,r.twitter_handle
        from restaurant r
        join city c
        on c.id = r.city_id
        where c.id = $cityId
         */
        $result = ORM::for_table('restaurant')
                ->select_many('restaurant.id','restaurant.name','restaurant.store_name','restaurant.address',
                        'restaurant.zip_code','restaurant.phone','restaurant.fb_page','restaurant.twitter_handle',
                        array('city_name' => 'city.name'))
                ->join('city', array('restaurant.city_id', '=', 'city.id'))
                ->where('city.id', $cityId)
                ->find_many();
        if($result) {
            $allRestaurantsInCity = array();
            foreach($result as $row) {
                $foundRestaurant = new \truefit\model\Restaurant();
                $foundRestaurant->id = $row->id;
                $foundRestaurant->name = $row->name;
                $foundRestaurant->storeName = $row->store_name;
                $foundRestaurant->address = $row->address;
                $foundRestaurant->zipCode = $row->zip_code;
                $foundRestaurant->phone = $row->phone;
                $foundRestaurant->facebookPage = $row->fb_page;
                $foundRestaurant->twitterHandle = $row->twitter_handle;
                $foundRestaurant->city = $row->city_name;
                $foundRestaurant->cityId = $cityId;
                $allRestaurantsInCity[] = $foundRestaurant;
            }
            return $allRestaurantsInCity;
        } else
            return null;
    }


    public function addRestaurant(\truefit\model\Restaurant $newRest) {
        try {
            if(!$this->validateZipCode($newRest->zipCode))
                return \truefit\APIErrorCodes::InvalidData;
            if(!$this->validatePhoneNumber($newRest->phone))
                return \truefit\APIErrorCodes::InvalidData;
            $newRecord = ORM::for_table("restaurant")->create();
            $newRecord->name = $newRest->name;
            $newRecord->store_name = $newRest->storeName;
            $newRecord->city_id = $newRest->cityId;
            $newRecord->address = $newRest->address;
            $newRecord->zip_code = $newRest->zipCode;
            $newRecord->phone = $newRest->phone;
            $newRecord->fb_page = $newRest->facebookPage;
            $newRecord->twitter_handle = $newRest->twitterHandle;
            $newRecord->save();
            $newRest->id = $newRecord->id();
            if(!is_null($newRest->hours) && is_array($newRest->hours)) {
                $this->addBussinesHour($newRest->hours, $newRest->id);
            }
            return $newRest->id;
        } catch (Exception $ex) {
            return \truefit\APIErrorCodes::General;
        }
    }
    
    public function addBussinesHour($newBussHours, $restaurantId) {
        foreach($newBussHours as $businessHour) {
            $newRecord = ORM::for_table("hours")->create();
            $newRecord->restaurant_id = $restaurantId;
            $newRecord->day = $businessHour->day;
            $newRecord->open_hour = $businessHour->openHour;
            $newRecord->close_hour = $businessHour->closeHour;
            $newRecord->save();
        }
    }
}

?>
