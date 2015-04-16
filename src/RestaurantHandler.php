<?php

/**
 * Description of restaurant
 *
 * @author john
 */

namespace truefit;

class RestaurantHandler extends HandlerBase {
    function __construct($payload = null) {
        parent::__construct();
        if($payload)
            $this->payload = $payload;
        else
            $this->payload = $this->app->request->getBody();
    }
    
    /* Base Methods */
    
    public function listAll() {
        throw new Exception("Not implemented");
    }

    public function add() {
        $response = new model\Response();
        try {
            $newRestaurant = new model\Restaurant($this->payload);
            $restaurantDb = new dal\RestaurantDB();
            $newId = $restaurantDb->addRestaurant($newRestaurant);
            
            if($newId > -1) {
                $response->success = true;
                $response->msg = "New restaurant added successfully";
            } else {
                $response->success = false;
                $response->msg = \truefit\APIErrorCodes::$msg[$newId];
            }
        } catch(\InvalidArgumentException $ex) {
            $response->success = false;
            $response->msg = \truefit\APIErrorCodes::$msg[-2];
        } catch(\Exception $ex) {
            $response->success = false;
            $response->msg = \truefit\APIErrorCodes::$msg[-1];
        }
        $this->app->contentType('application/json');
        $this->app->response->body(json_encode($response));
    }
    
    public function remove() {
        throw new Exception("Not implemented");
    }
    
    public function details($restaurantId) {
        $response = new model\Response();
        try {
            $restaurantDb = new dal\RestaurantDB();
            $restaurantDetails = $restaurantDb->byId($restaurantId);
            $response->success = true;
            $response->msg = "Details of restaurant ".$restaurantId;
            $response->details = $restaurantDetails;
        } catch(\Exception $ex) {
            $response->success = false;
            $response->msg = \truefit\APIErrorCodes::$msg[-1];
        }
        $this->app->contentType('application/json');
        $this->app->response->body(json_encode($response));
    }
    
    /* End Base Methods */
    
    
    /* Specific Methods */
    
    public function listByCity($cityId) {
        $response = new model\Response();
        try {
            $restaurantDb = new dal\RestaurantDB();
            $allInCity = $restaurantDb->byCity($cityId);
            $response->success = true;
            $response->msg = "List of restaurants in city ".$cityId;
            $response->details = $allInCity;
        }catch(\Exception $ex) {
            $response->success = false;
            $response->msg = \truefit\APIErrorCodes::$msg[-1];
        }
        $this->app->contentType('application/json');
        $this->app->response->body(json_encode($response));
    }
    
    /* End Specific Methods */
}

?>