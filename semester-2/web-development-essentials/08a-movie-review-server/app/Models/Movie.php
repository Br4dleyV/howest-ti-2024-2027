<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Concerns\HasUuids;
use Illuminate\Database\Eloquent\Model;

class Movie extends Model
{
    use HasUuids;

    public function reviews()
    {
        return $this->hasMany(Review::class);
    }
}
