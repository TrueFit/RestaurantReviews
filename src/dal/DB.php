<?php
namespace truefit\dal;

use \ORM;

use DateTime;

abstract class DB
{
    protected $db;

    public function __construct()
    {
        require 'orm.config.php';
        ORM::configure($ormConfig);
    }
    
    function validateZipCode($zipCode) {
        //not implemented
        return true;
    }
    
    function validatePhoneNumber($phoneNumber) {
        //not implemented
        return true;
    }
    
    function validateDate($date)
    {
        $d = DateTime::createFromFormat('Y-m-d', $date);
        return $d && $d->format('Y-m-d') == $date;
    }
    
    function isExists($table_name, $column_name, $value) {
        return $result = \ORM::for_table($table_name)
                ->select($column_name)
                ->where($column_name, $value)
                ->find_one();
    }
}
?>