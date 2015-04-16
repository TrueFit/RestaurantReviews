<?php
// LOCALHOST
$ormConfig = array(
  'connection_string' => 'mysql:host=127.0.0.1;dbname=restaurantreviews',
  'username' => 'root',
  'password' => 'root',
  'driver_options' => array(
    PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES utf8'
  ),
  'logging' => true,
  'id_column_overrides', array()
);
?>