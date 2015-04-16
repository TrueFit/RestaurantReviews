<?php
/**
 * Description of HandlerBase
 *
 * @author john
 */

namespace truefit;

abstract class HandlerBase {
    protected $app;
    protected $payload;
            
    function __construct() {
        $this->app = \Slim\Slim::getInstance();
    }
    
    abstract function listAll();
    
    abstract function add();
    
    abstract function remove();
    
    abstract function details($id);
}

?>
