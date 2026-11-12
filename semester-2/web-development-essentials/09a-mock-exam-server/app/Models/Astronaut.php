<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Concerns\HasUuids;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Astronaut extends Model
{
    use HasFactory, HasUuids;

    protected $fillable = [
        'name',
        'endurance',
        'skill',
    ];

    public function missions()
    {
        return $this->belongsToMany(Mission::class);
    }
}