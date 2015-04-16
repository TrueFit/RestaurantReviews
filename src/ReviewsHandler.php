<?php
/**
 * Description of ReviewsHandler
 *
 * @author john
 */
namespace truefit;

class ReviewsHandler extends HandlerBase {
    function __construct() {
        parent::__construct();
        $this->payload = $this->app->request->getBody();
    }
    
    /* Base Methods */
    
    public function listAll() {
        throw new Exception("Not implemented");
    }

    public function add() {
        $response = new model\Response();
        try {
            $newReview = new model\Review($this->payload);
            $reviewDb = new dal\ReviewDB();
            $newId = $reviewDb->addReview($newReview);
            if($newId > -1) {
                $response->success = true;
                $response->msg = "New review added successfully";
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
        $response = new model\Response();
        try {
            $jsonDelete = json_decode($this->payload);
            if($jsonDelete) {
                $reviewDb = new dal\ReviewDB();
                $result = $reviewDb->deleteReview($jsonDelete->{'id'});
                if($result > -1) {
                    $response->success = true;
                    $response->msg = "Review deleted successfully";
                } else {
                    $response->success = false;
                    $response->msg = \truefit\APIErrorCodes::$msg[$result];
                }
            } else {
                $response->success = false;
                $response->msg = \truefit\APIErrorCodes::$msg[-2];
            }
        } catch(\Exception $ex) {
            $response->success = false;
            $response->msg = \truefit\APIErrorCodes::$msg[-1];
        }
        $this->app->contentType('application/json');
        $this->app->response->body(json_encode($response));
    }
    
    public function details($reviewId) {
        throw new Exception("Not implemented");
    }
    
    /* End Base Methods */
    
    /* Specific Methods */
    
    public function listByUser($userId) {
        $response = new model\Response();
        try {
            $reviewDb = new dal\ReviewDB();
            $allByUser = $reviewDb->byUser($userId);
            $response->success = true;
            $response->msg = "List of reviews by user ".$userId;
            $response->details = $allByUser;
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
