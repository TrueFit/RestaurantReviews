<?php
/**
 * Description of APIMessages
 *
 * @author john
 */
namespace truefit;

class APIErrorCodes {
    const General = -1;
    const InvalidData = -2;
    const NotFound = -3;
    
    static $msg = Array (
        -1 => "Unknown error",
        -2 => "API received incorrect data",
        -3 => "Not found"
    );
}

?>
