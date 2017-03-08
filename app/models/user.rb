class User < ApplicationRecord
  has_many :reviews

  before_create -> { self.auth_token = SecureRandom.hex }
end
