Rails.application.routes.draw do
  resources :reviews
  resources :restaurants
  resources :users
  # For details on the DSL available within this file, see http://guides.rubyonrails.org/routing.html

  get :token, controller: 'application'
end