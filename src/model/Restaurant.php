<?php
/**
 * Description of Restaurant
 *
 * @author john
 */
namespace truefit\model;


class Restaurant {
    public $id;
    public $name;
    public $storeName;
    public $cityId;
    public $city;
    public $address;
    public $zipCode;
    public $phone;
    public $facebookPage;
    public $twitterHandle;
    public $hours;
    
    public function __construct($json = false) {
        if($json)
            $this->set(json_decode($json,true));
    }
    
    public function set($data) {
        if($data == null) {
            throw new \InvalidArgumentException;
        }
        foreach($data as $key => $value) {
            if($key == "hours" && is_array($value)) {
                foreach($value as $row) {
                    $bussHours = new BusinessHour();
                    $bussHours->day = $row["day"];
                    $bussHours->openHour = $row["openHour"];
                    $bussHours->closeHour = $row["closeHour"];
                    $this->hours[] = $bussHours;
                }
            } else {
                $this->$key = $value;
            }
        }
    }
}

?>
