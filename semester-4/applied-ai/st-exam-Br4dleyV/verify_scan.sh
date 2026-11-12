#!/bin/bash
# Send request
curl -X POST \
  http://localhost:8000/api/assistant/scan-hackathon-registration-form \
  -F "registrationFormImage=@hackathon-registration-form.jpg" \
  -v
