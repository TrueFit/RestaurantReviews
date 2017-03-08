class CreateUsers < ActiveRecord::Migration[5.0]
  def change
    create_table :users do |t|
      t.string :email
      t.string :display_name
      t.string :password
      t.string :auth_token

      t.timestamps
    end
  end
end
