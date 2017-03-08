class UsersController < ApplicationController
  before_action :set_user, only: [:show, :update, :destroy]

  # GET /users - returns all users
  def index
    @users = User.all

    render json: @users
  end

  # GET /users/:user_id - returns a particular user
  def show
    render json: @user
  end

  # POST /users - creates a new user
  def create
    @user = User.new(user_params)

    if @user.save
      render json: @user, status: :created, location: @user
    else
      render json: @user.errors, status: :unprocessable_entity
    end
  end

  # PATCH/PUT /users/:user_id - updates a user
  def update
    if @user.update(user_params)
      render json: @user
    else
      render json: @user.errors, status: :unprocessable_entity
    end
  end

  # DELETE /users/:user_id - deletes a user
  def destroy
    @user.destroy
  end

  private
    def set_user
      @user = User.find(params[:id])
    end

    def user_params
      params.require(:user).permit(:email, :display_name, :password, :auth_token)
    end
end
