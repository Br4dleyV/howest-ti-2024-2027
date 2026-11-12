<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Concerns\HasUuids;
use Illuminate\Database\Eloquent\Model;

class Destination extends Model
{
    use HasUuids;
    protected $fillable = [
        "name",
        "description",
        "distance",
        "duration",
    ];
    public function missions()
    {
        return $this->hasMany(Mission::class);
    }
    public function astronauts()
    {
        return $this->hasManyThrough(Astronaut::class, Mission::class);
    }
}
