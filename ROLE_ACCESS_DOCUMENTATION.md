# Role Access Documentation

This document lists all API methods and who can access each one, based on the current controller attributes.

## Access Rules Legend

- `Public`: no `[Authorize]` attribute, accessible without authentication.
- `Authenticated`: `[Authorize]` without role restriction, any logged-in user.
- `Player`: `[Authorize(Roles = DefaultRoles.Player)]`.
- `FieldOwner`: `[Authorize(Roles = DefaultRoles.FieldOwner)]`.
- `Admin`: `[Authorize(Roles = DefaultRoles.Admin)]`.
- `Player or Admin`: `[Authorize(Roles = $"{DefaultRoles.Player},{DefaultRoles.Admin}")]`.

---

## AuthController (`/Auth`)

- `POST /Auth` -> `Public`
- `POST /Auth/refresh` -> `Public`
- `PUT /Auth/revoke-refresh-token` -> `Public`
- `POST /Auth/player-register` -> `Public`
- `POST /Auth/field-owner-register` -> `Public`
- `POST /Auth/confirm-email` -> `Public`
- `POST /Auth/resend-confirmation-email` -> `Public`
- `POST /Auth/forget-password` -> `Public`
- `POST /Auth/verify-reset-password-otp` -> `Public`
- `POST /Auth/reset-password` -> `Public`

## AccountController (`/Account`)

- `GET /Account/player/{userId}` -> `Authenticated`
- `GET /Account/field-owner` -> `FieldOwner`
- `PUT /Account/player/change-password` -> `Player`
- `PUT /Account/field-owner/change-password` -> `FieldOwner`
- `DELETE /Account/player` -> `Player`
- `DELETE /Account/field-owner` -> `FieldOwner`
- `PUT /Account/player` -> `Player`
- `PUT /Account/field-owner` -> `FieldOwner`

## AdminController (`/Admin`)

- `GET /Admin/bookings` -> `Admin`
- `GET /Admin/invitations` -> `Admin`
- `GET /Admin/field-owners` -> `Admin`
- `PUT /Admin/field-owners/{userId}/status` -> `Admin`

## FieldController (`/Field`)

- `GET /Field/get-all` -> `Authenticated`
- `GET /Field/get-by-id/{id}` -> `Authenticated`
- `GET /Field/get-by-owner-id/{ownerId}` -> `Authenticated`
- `POST /Field` -> `FieldOwner`
- `DELETE /Field/{id}` -> `FieldOwner`
- `PUT /Field/{id}` -> `FieldOwner`

## FieldSlotController (`/FieldSlot`)

- `POST /FieldSlot/{fieldId}` -> `FieldOwner`
- `PUT /FieldSlot/{fieldId}/slot/{id}` -> `FieldOwner`
- `DELETE /FieldSlot/{fieldId}/slot/{id}` -> `FieldOwner`
- `GET /FieldSlot/{id}` -> `Authenticated`
- `GET /FieldSlot/field/{fieldId}` -> `Authenticated`
- `GET /FieldSlot/field/{fieldId}/avialable-slots` -> `Authenticated`

## FieldReviewController (`/FieldReview`)

- `POST /FieldReview/{fieldId}` -> `Player`
- `PUT /FieldReview/{reviewId}` -> `Player`
- `DELETE /FieldReview/{reviewId}` -> `Player or Admin`
- `GET /FieldReview/{reviewId}` -> `Authenticated`
- `GET /FieldReview/{fieldId}/reviews` -> `Authenticated`

## FollowController (`/Follow`)

- `POST /Follow` -> `Authenticated`
- `DELETE /Follow` -> `Authenticated`
- `GET /Follow/{userId}/followers` -> `Authenticated`
- `GET /Follow/{userId}/following` -> `Authenticated`
- `POST /Follow/follow/{email}` -> `Authenticated` (placeholder method currently returns `NoContent`)

## InvitationController (`/Invitation`)

- `POST /Invitation/Invite` -> `Player`
- `POST /Invitation/Request/{fieldSlotId}` -> `Player`
- `POST /Invitation/Accept/{invitationId}` -> `Player`
- `POST /Invitation/Reject/{invitationId}` -> `Player`
- `DELETE /Invitation/{id}` -> `Player`
- `GET /Invitation/recieved` -> `Player`
- `GET /Invitation/sent` -> `Player`

## BookingController (`/Booking`)

- `POST /Booking/{fieldSlotId}` -> `Player`
- `DELETE /Booking/{bookingId}` -> `Player`
- `GET /Booking/{bookingId}` -> `Player`
- `GET /Booking/my` -> `Player`

## FieldOwnerController (`/FieldOwner`)

- `GET /FieldOwner/fields` -> `FieldOwner`
- `GET /FieldOwner/bookings` -> `FieldOwner`
- `GET /FieldOwner/invitations` -> `FieldOwner`

---

## Maintenance Note

When adding a new endpoint:

- Add/verify `[Authorize]` and the required `Roles` value.
- Update this file in the same PR to keep access documentation synchronized with code.
