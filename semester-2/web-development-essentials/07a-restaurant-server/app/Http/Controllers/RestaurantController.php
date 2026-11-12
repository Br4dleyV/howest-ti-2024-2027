<?php

namespace App\Http\Controllers;

use App\Models\Restaurant;
use App\Models\Booking;
use Illuminate\Http\Request;

class RestaurantController extends Controller
{
    function getAllRestaurants() {
        return [ "data" => Restaurant::all() ];
    }

    function getBookings(Restaurant $restaurant) {
        return [ "data" => $restaurant -> bookings ];
    }

    function addBooking(Request $request) {
        $booking = new Booking($request -> all());
        $booking -> save();

        return $booking;
    }
}
