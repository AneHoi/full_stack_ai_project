CREATE TABLE allergenedb.user_allergens (
    user_id INT REFERENCES allergenedb.users(id),
    category_id INT REFERENCES allergenedb.categories(id),
    PRIMARY KEY (user_id, category_id)
);