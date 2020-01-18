import React from 'react'
import { TextField, Paper, Grid, Button, Typography, Select, MenuItem, InputLabel, FormControl } from '@material-ui/core'
import FormWrapper from '../../../components/forms/FormWrapper'
import BusinessIcon from '@material-ui/icons/Business'
import ContactMailIcon from '@material-ui/icons/ContactMail'
import VpnKeyIcon from '@material-ui/icons/VpnKey'

import './AccountInformation.less'

export interface AccountInformationProps {}

class AccountInformation extends React.Component<AccountInformationProps> {
    render() {
        return (
            <React.Fragment>
                <Paper className="paper wide">
                    <Typography variant="h2" className="override-h2" component="h2">
                        <VpnKeyIcon /> Credentials
                    </Typography>

                    <FormWrapper>
                        <Grid container spacing={3}>
                            <Grid item xs={6}>
                                <TextField name="email" label="Email" fullWidth />
                            </Grid>
                        </Grid>

                        <Grid container spacing={3}>
                            <Grid item xs={6}>
                                <TextField name="password" type="password" label="Password" fullWidth />
                            </Grid>
                            <Grid item xs={6}>
                                <TextField name="password-confirmation" type="password" label="Password confirmation" fullWidth />
                            </Grid>
                        </Grid>
                    </FormWrapper>

                    <Grid container spacing={3}>
                        <Grid item xs={4}>
                            <Button variant="contained" type="submit" color="primary" disabled={false}>
                                Update
                            </Button>
                        </Grid>
                    </Grid>
                </Paper>

                <Grid container spacing={3}>
                    <Grid item xs={12} lg={6}>
                        <Paper className="paper">
                            <Typography variant="h2" className="override-h2" component="h2">
                                <ContactMailIcon /> Contact information
                            </Typography>

                            <FormWrapper>
                                <Grid container spacing={3}>
                                    <Grid item xs={6}>
                                        <TextField name="firstName" label="First Name" fullWidth />
                                    </Grid>
                                    <Grid item xs={6}>
                                        <TextField name="lastName" label="Last Name" fullWidth />
                                    </Grid>
                                </Grid>

                                <Grid container spacing={3}>
                                    <Grid item xs={6}>
                                        <TextField name="email" label="Email" fullWidth />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField name="phone" label="Phone" fullWidth />
                                    </Grid>
                                </Grid>

                                <Grid container spacing={3}>
                                    <Grid item xs={3}>
                                        <FormControl fullWidth>
                                            <InputLabel id="country-label">Country</InputLabel>
                                            <Select labelId="country-label" id="country" name="country" fullWidth>
                                                <MenuItem value={'CA'}>Canada</MenuItem>
                                            </Select>
                                        </FormControl>
                                    </Grid>
                                    <Grid item xs={3}>
                                        <FormControl fullWidth>
                                            <InputLabel id="state-label">State</InputLabel>
                                            <Select labelId="state-label" id="state" name="state" fullWidth>
                                                <MenuItem value={'CA'}>Quebec</MenuItem>
                                            </Select>
                                        </FormControl>
                                    </Grid>
                                    <Grid item xs={6}>
                                        <TextField name="city" label="City" fullWidth />
                                    </Grid>
                                </Grid>

                                <Grid container spacing={3}>
                                    <Grid item xs={6}>
                                        <TextField name="address" label="Address" fullWidth />
                                    </Grid>
                                    <Grid item xs={3}>
                                        <TextField name="zipcode" label="Zip/Postal code" fullWidth />
                                    </Grid>
                                </Grid>
                            </FormWrapper>

                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Button variant="contained" type="submit" color="primary" disabled={false}>
                                        Update
                                    </Button>
                                </Grid>
                            </Grid>
                        </Paper>
                    </Grid>

                    <Grid item xs={6}>
                        <Paper className="paper">
                            <Typography variant="h2" className="override-h2" component="h2">
                                <BusinessIcon /> Business information
                            </Typography>

                            <FormWrapper>
                                <Grid container spacing={3}>
                                    <Grid item xs={12}>
                                        <TextField name="business-name" label="Name" fullWidth />
                                    </Grid>
                                </Grid>
                            </FormWrapper>

                            <Grid container spacing={3}>
                                <Grid item xs={4}>
                                    <Button variant="contained" type="submit" color="primary" disabled={false}>
                                        Update
                                    </Button>
                                </Grid>
                            </Grid>
                        </Paper>
                    </Grid>
                </Grid>
            </React.Fragment>
        )
    }
}

export default AccountInformation
