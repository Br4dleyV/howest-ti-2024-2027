<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Concerns\HasUuids;
use Illuminate\Database\Eloquent\Model;

class Booking extends Model
{
    use HasUuids;

    protected $fillable = [
        'name',
        'email',
        'dateTime',
        'people',
        'restaurant_id',
    ];

    public function restaurant() {
        return $this->belongsTo(Restaurant::class);
    }
}