<?php
/**
 * Description of CityDB
 *
 * @author john
 */
namespace truefit\dal;

use \ORM;

class CityDB extends DB {
    
    public function __construct()
    {
        parent::__construct();
    }
    
    public function byId($cityId) {
        $result = ORM::for_table('city')->select_many('city.name','city.state_id','city.country_id',
                'city.latitude','city.longitude',array('state_name' => 'state.name'),
                array('country_name' => 'country.name'))
                ->where_raw('(state.country_id = country.id or city.country_id = country.id) 
                    and city.state_id = state.id and city.id = ?', array($cityId))
                ->find_one();
        if($result) {
            $foundCity = new \truefit\model\City();
            $foundCity->id = $cityId;
            $foundCity->name = $result->name;
            $foundCity->stateId = $result->state_id;
            $foundCity->state = $result->state_name;
            $foundCity->countryId = $result->country_id;
            $foundCity->country = $result->country_name;
            $foundCity->latitude = $result->latitude;
            $foundCity->longitude = $result->longitude;
            return $foundCity;
        } else
            return null;
    }
    
    public function addCity(\truefit\model\City $newCity) {
        $cityRow = ORM::for_table('city')->create();
        $cityRow->name = $newCity->name;
        $cityRow->state_id = $newCity->stateId;
        $cityRow->country_id = $newCity->countryId;
        $cityRow->latitude = $newCity->latitude;
        $cityRow->longitude = $newCity->longitude;
        $cityRow->save();
        return $cityRow->id();
    }
}

?>
