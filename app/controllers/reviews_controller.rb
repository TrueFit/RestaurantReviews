class ReviewsController < ApplicationController
  before_action :set_review, only: [:show, :update, :destroy]

  # GET /reviews - returns all reviews, or all reviews for a particular user if a user_id is specified
    def index
      if params[:user_id].blank?
        @reviews = Review.all
      else
        @reviews = Review.where(user_id: params[:user_id])
      end

      render json: @reviews
    end

  # GET /reviews/:review_id - returns a review given its ID
  def show
    render json: @review
  end

  # POST /reviews - creates a new review
  def create
    @review = Review.new(review_params)

    if @review.save
      render json: @review, status: :created, location: @review
    else
      render json: @review.errors, status: :unprocessable_entity
    end
  end

  # PATCH/PUT /reviews/:review_id - updates a particular review
  def update
    if @review.update(review_params)
      render json: @review
    else
      render json: @review.errors, status: :unprocessable_entity
    end
  end

  # DELETE /reviews/:review_id - deletes a review (users are only able to delete their own reviews)
  def destroy
    @review.destroy
  end

  private
    def set_review
      @review = Review.find(params[:id])
    end

    def review_params
      params.require(:review).permit(:title, :body, :rating, :user_id, :restaurant_id)
    end
end
