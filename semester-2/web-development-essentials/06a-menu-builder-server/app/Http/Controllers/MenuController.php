<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Arr;

class MenuController extends Controller
{
    public function random() {
        $dishes = [ "beef", "chicken", "cod", "couscous", "eggrolls", "gazpacho", "kebab", "linguine", "meatballs", "pork", "quesadillas", "salad", "salmon", "sandwich", "shrimp", "soup", "stirfry", "tacos", "zucchini" ];

        $randomDishes = Arr::random($dishes, 2);

        return response()->json([
            'menu' => [
                'starter' => $randomDishes[0],
                'main' => $randomDishes[1],
                'dessert' => 'icecream'
            ]
        ]);
    }
}
