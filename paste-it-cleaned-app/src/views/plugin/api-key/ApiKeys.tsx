import React, { Fragment } from 'react'
import VpnKeyIcon from '@material-ui/icons/VpnKey'
import AddIcon from '@material-ui/icons/Add'
import RemoveIcon from '@material-ui/icons/Remove'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'

import { Paper, Typography, Grid, FormControlLabel, Switch, Chip, Button, LinearProgress } from '@material-ui/core'

import './ApiKeys.less'
import { ApiKeyEntity } from '../../../entities/api'
import { ApiKeysController } from './ApiKeysController'
import ButtonWithLoading from '../../../components/ButtonWithLoading'
import LoadingError from '../../../components/LoadingError'
import { Skeleton } from '@material-ui/lab'

export interface ApiKeysProps {}
export interface ApiKeysState {
    isLoaded: boolean
    apiKeys: ApiKeyEntity[]
    apiKeysLoading: boolean
    apiKeysError: boolean
    apiKeyLoading: boolean
    apiKeyError: boolean
}

class ApiKeys extends React.Component<ApiKeysProps, ApiKeysState> {
    private controller?: ApiKeysController = undefined

    constructor(props: ApiKeysProps) {
        super(props)
        this.controller = new ApiKeysController(this)
        this.state = {
            isLoaded: false,
            apiKeys: [],
            apiKeysLoading: false,
            apiKeysError: false,
            apiKeyLoading: false,
            apiKeyError: false,
        }
    }

    componentDidMount() {
        this.controller!.initialize()
    }

    private handleDeleteDomain() {
        // gérer la clé associée
    }

    render() {
        return (
            <Fragment>
                {!this.state.isLoaded && <LinearProgress />}
                <Typography variant="h2" className="override-h2" component="h2">
                    <VpnKeyIcon /> Api keys
                </Typography>
                <br />
                {this.state.apiKeysError ? (
                    <React.Fragment>
                        <LoadingError />
                    </React.Fragment>
                ) : (
                    <React.Fragment>
                        <ButtonWithLoading loading={this.state.apiKeyLoading} variant="contained" color="primary" onClick={this.controller!.createApiKey}>
                            Generate new key
                        </ButtonWithLoading>

                        {this.state.apiKeysLoading ? (
                            <React.Fragment>
                                <Skeleton height={200} className="paper paper-no-padding" />
                                <Skeleton height={200} className="paper paper-no-padding" />
                            </React.Fragment>
                        ) : (
                            <React.Fragment>
                                {this.state.apiKeys &&
                                    this.state.apiKeys.map(key => (
                                        <Paper className="paper paper-no-padding">
                                            <Grid container spacing={1}>
                                                <Grid item xs={12} md={2} className="api-key-card-left grid-paper-padding">
                                                    <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                                        <Grid item>
                                                            <Chip size="small" label="Active" className="chip-green" />
                                                        </Grid>
                                                        <Grid item>
                                                            <Typography variant="caption" className="override-caption" component="span" style={{ color: '#ddd' }}>
                                                                Expires on{' '}
                                                                <span style={{ whiteSpace: 'nowrap' }}>
                                                                    2019-02-15 <EditIcon onClick={() => {}} cursor="pointer" />
                                                                </span>
                                                            </Typography>
                                                        </Grid>
                                                        <Grid item>
                                                            <Button color="secondary" size="small" variant="contained">
                                                                <RemoveIcon /> Delete key
                                                            </Button>
                                                        </Grid>
                                                    </Grid>
                                                </Grid>
                                                <Grid item xs={12} md={10} className="grid-paper-padding">
                                                    <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                                        <Grid item style={{ height: '35px', width: '100%' }}>
                                                            <Grid container justify="space-between" alignItems="flex-start" style={{ width: '100%', display: 'flex' }}>
                                                                <Grid item style={{ verticalAlign: 'middle' }}>
                                                                    <Typography variant="h3" className="override-h2" component="h3">
                                                                        <VpnKeyIcon />
                                                                        2354b4968e774dbe8
                                                                    </Typography>
                                                                </Grid>
                                                                <Grid item style={{ alignSelf: 'flex-start', paddingRight: '5px' }}>
                                                                    <FormControlLabel labelPlacement="start" control={<Switch checked={true} value="checkedB" color="primary" size="small" />} label="Enabled" />
                                                                </Grid>
                                                            </Grid>
                                                        </Grid>
                                                        <Grid item>
                                                            <Chip size="small" variant="default" color="primary" label="commerscale.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                                            <Chip size="small" variant="default" color="primary" label="asdf.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                                            <Chip size="small" variant="default" color="primary" label="fffff.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                                            <Chip size="small" variant="default" color="primary" label="eeeeee.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                                            <Chip size="small" variant="default" color="primary" label="sasdfewefsdg.com" className="chip-spaced" deleteIcon={<DeleteIcon />} onDelete={this.handleDeleteDomain} />
                                                        </Grid>
                                                        <Grid item style={{ height: '45px', display: 'flex' }}>
                                                            <Button color="primary" size="small" variant="outlined" style={{ alignSelf: 'flex-end' }}>
                                                                <AddIcon /> Add domain
                                                            </Button>
                                                        </Grid>
                                                    </Grid>
                                                </Grid>
                                            </Grid>
                                        </Paper>
                                    ))}
                            </React.Fragment>
                        )}
                    </React.Fragment>
                )}
            </Fragment>
        )
    }
}

export default ApiKeys
