<?php

namespace App\Http\Controllers;

use App\Models\Astronaut;

class AstronautController extends Controller
{
    public function getAllAstronauts() {
        return [ "data" => Astronaut::all()];
    }
}