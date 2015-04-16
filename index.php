<?php
require 'vendor/autoload.php';

$app = new \Slim\Slim(array(
    'log.level' => \Slim\Log::DEBUG
));

$app->add(new \Slim\Middleware\ContentTypes());

$app->get('/', function() use ($app) {
    $app->halt(403, "<b>403 Forbidden</b>");
});

$app->group('/restaurants', function () use ($app) {
    $app->get('/city/:id', 'truefit\RestaurantHandler:listByCity');
    $app->post('/', 'truefit\RestaurantHandler:add');
    $app->get('/:id', 'truefit\RestaurantHandler:details');
});

$app->group('/reviews', function () use ($app) {
    $app->get('/user/:id', 'truefit\ReviewsHandler:listByUser');
    $app->post('/', 'truefit\ReviewsHandler:add');
    $app->delete('/', 'truefit\ReviewsHandler:remove');
});

$app->group('/test', function () use ($app) {
    $app->get('/RestaurantTests', 'truefitTests\RestaurantTests:runAll');
    $app->get('/RestaurantTests/add', 'truefitTests\RestaurantTests:addTest');
    $app->get('/RestaurantTests/details', 'truefitTests\RestaurantTests:detailsTest');
});
$app->run();
?>