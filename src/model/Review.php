<?php
/**
 * Description of Review
 *
 * @author john
 */
namespace truefit\model;

class Review {
    public $id;
    public $reviewByUser;
    public $reviewByUserId;
    public $restaurantName;
    public $restaurantId;
    public $reviewBody;
    public $reviewTime;
    public $stars;
    
    public function __construct($json = false) {
        if($json)
            $this->set(json_decode($json,true));
    }
    
    public function set($data) {
        if($data == null) {
            throw new \InvalidArgumentException;
        }
        foreach($data as $key => $value) {
            $this->$key = $value;
        }
    }
}

?>
