<?php
/**
 * Description of ReviewDB
 *
 * @author john
 */
namespace truefit\dal;

use \ORM;
use Exception;

class ReviewDB extends DB {
    public function __construct()
    {
        parent::__construct();
    }
    
    public function byUser($userId) {
        /*
        SELECT rev.id,rev.user_id,rev.restaurant_id,rev.review,rev.review_time,rev.stars, u.fname, u.lname, r.name
        FROM restaurantreviews.reviews rev
        join users u
        on rev.user_id = u.id
        join restaurant r
        on rev.restaurant_id = r.id
        where rev.user_id = $userId
         */
        
        $result = ORM::for_table('reviews')
                ->select_many('reviews.id','reviews.user_id','reviews.restaurant_id','reviews.review',
                        'reviews.review_time','reviews.stars','users.fname','users.lname', 'restaurant.name')
                ->join('users', array('reviews.user_id', '=', 'users.id'))
                ->join('restaurant', array('reviews.restaurant_id', '=', 'restaurant.id'))
                ->where('reviews.user_id', $userId)
                ->find_many();
        if($result) {
            $allReviewsByUser = array();
            foreach($result as $row) {
                $foundReview = new \truefit\model\Review();
                $foundReview->id = $row->id;
                $foundReview->reviewByUser = $row->fname .' '. $row->lname;
                $foundReview->reviewByUserId = $row->user_id;
                $foundReview->restaurantId = $row->restaurant_id;
                $foundReview->restaurantName = $row->name;
                $foundReview->reviewBody = $row->review;
                $foundReview->reviewTime = $row->review_time;
                $foundReview->stars = $row->stars;
                $allReviewsByUser[] = $foundReview;
            }
            return $allReviewsByUser;
        } else
            return null;
    }
    
    function addReview(\truefit\model\Review $newReview) {
        try {
            if(!$this->validateDate($newReview->reviewTime))
                return \truefit\APIErrorCodes::InvalidData;
            if($newReview->stars < 1 || $newReview->stars > 5)
                return \truefit\APIErrorCodes::InvalidData;
            $newRecord = ORM::for_table("reviews")->create();
            $newRecord->user_id = $newReview->reviewByUserId;
            $newRecord->restaurant_id = $newReview->restaurantId;
            $newRecord->review = $newReview->reviewBody;
            $newRecord->review_time = $newReview->reviewTime;
            $newRecord->stars = $newReview->stars;
            $newRecord->save();
            $newReview->id = $newRecord->id();
            return $newReview->id;
        } catch(\Exception $ex) {
            return \truefit\APIErrorCodes::General;
        }
    }
    
    function deleteReview($id) {
        try {
            $found = $this->isExists('reviews', 'id', $id);
            if($found)
            {
                $found->delete();
                return 0;
            } else {
                return \truefit\APIErrorCodes::NotFound;
            }
        } catch(\Exception $ex) {
            return \truefit\APIErrorCodes::General;
        }
    }
}

?>
