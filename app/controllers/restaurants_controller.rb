class RestaurantsController < ApplicationController
  before_action :set_restaurant, only: [:show, :update, :destroy]

  # GET /restaurants - returns all restaurants, or all restaurants for a particular city if specified
  def index
    if params[:city].blank?
      @restaurants = Restaurant.all
    else
      @restaurants = Restaurant.where(location: params[:city])
    end

    render json: @restaurants
  end

  # GET /restaurants/:restaurant_id - returns a restaurant given its ID
  def show
    render json: @restaurant
  end

  # POST /restaurants - creates a new restaurant
  def create
    @restaurant = Restaurant.new(restaurant_params)

    if @restaurant.save
      render json: @restaurant, status: :created, location: @restaurant
    else
      render json: @restaurant.errors, status: :unprocessable_entity
    end
  end

  # PATCH/PUT /restaurants/:restaurant_id - updates a particular restaurant
  def update
    if @restaurant.update(restaurant_params)
      render json: @restaurant
    else
      render json: @restaurant.errors, status: :unprocessable_entity
    end
  end

  # DELETE /restaurants/:restaurant_id - deletes a restaurant
  def destroy
    @restaurant.destroy
  end

  private
    def set_restaurant
      @restaurant = Restaurant.find(params[:id])
    end

    def restaurant_params
      params.require(:restaurant).permit(:name, :location)
    end
end
