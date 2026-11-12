<?php

use App\Http\Controllers\AstronautController;
use App\Http\Controllers\DestinationController;
use App\Http\Controllers\MissionController;
use App\Http\Middleware\ValidateToken;
use Illuminate\Support\Facades\Route;

Route::get("/destinations", [DestinationController::class, "getAllDestinations"]);
Route::get("/astronauts", [AstronautController::class, "getAllAstronauts"]);
Route::middleware([ValidateToken::class])->group(function () {
    Route::post("/execute-mission", [MissionController::class, "executeMission"]);
});