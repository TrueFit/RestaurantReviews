# Example test values to initially populate the db

user1 = User.create(email: 'user@test.com', display_name: 'user1', password: 'password')
user2 = User.create(email: 'user2@test.com', display_name: 'user2', password: 'password')

restaurant1 = Restaurant.create(name:'Gaucho', location:'Pittsburgh')
restaurant2 = Restaurant.create(name:'Smallman Galley', location:'Pittsburgh')
restaurant3 = Restaurant.create(name:'Wahlburgers', location:'Boston')

user1.reviews.create(title: 'A Gaucho Review', body: 'Gaucho was delicious.', rating: '10', restaurant: restaurant1, user: user1)
user2.reviews.create(title: 'Another Gaucho Review', body: 'I also though Gaucho was delicious!', rating: '9', restaurant: restaurant1, user: user2)
user1.reviews.create(title: 'My Smallman Galley Review', body: 'Smallman Galley had foods of the delicious variety.', rating: '8', restaurant: restaurant2, user: user1)
user2.reviews.create(title: 'A Humble Review of Smallman Galley', body: 'In my opinion, Smallman Galley = yes.', rating: '7', restaurant: restaurant2, user: user2)
user2.reviews.create(title: 'Best burgers ever!', body: 'Nothing beats a burger made by Donnie Wahlberg himself', rating: '6', restaurant: restaurant3, user: user2)
